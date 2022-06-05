using System.Collections.Concurrent;
using System.Text;
using ModU.Abstract.Modules;

namespace ModU.Infrastructure.Modules;

internal sealed class ModuleResolver : IModuleResolver
{
    private static readonly ConcurrentDictionary<string, IModule> Modules = new();
    private readonly ModuleNameResolver _moduleNameResolver = new();

    public IModule ResolveForType(Type type)
    {
        var moduleName = _moduleNameResolver.Resolve(type.FullName!);
        var module = Modules.GetOrAdd(moduleName, static (m, t) =>
        {
            var projectName = GetProjectName(t.FullName!);
            var moduleTypeName = $"{projectName}.Modules.{m}.Infrastructure.{m}Module";
            return (IModule) Activator.CreateInstance(Type.GetType(moduleTypeName)!)!;
        }, type);

        return module;
    }

    private static string GetProjectName(string typeFullName)
    {
        const char dot = '.';
        var builder = new StringBuilder();
        for (var i = 0; i < typeFullName.Length; i++)
        {
            var character = typeFullName[i];
            if (character == dot)
            {
                break;
            }
            
            builder.Append(character);
        }

        return builder.ToString();
    }
}