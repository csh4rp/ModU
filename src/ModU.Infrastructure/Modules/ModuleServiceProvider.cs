using System.Reflection;

namespace ModU.Infrastructure.Modules;

public class ModuleServiceProvider
{
    private readonly ModuleNameResolver _moduleNameResolver = new();
    private readonly IServiceProvider _serviceProvider;
    
    public TService Get<TService>(Type typeFromModule)
    {
        var moduleName = _moduleNameResolver.Resolve(typeFromModule.FullName!);
        return default;
    }
}