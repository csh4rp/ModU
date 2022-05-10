using ModU.Abstract.Modules;

namespace ModU.Infrastructure.Commands;

internal sealed class UnitOfWorkTypeRegistry
{
    private static readonly Dictionary<string, Type> Registry = new();
    private readonly IModuleNameResolver _moduleNameResolver;

    public UnitOfWorkTypeRegistry(IModuleNameResolver moduleNameResolver) => _moduleNameResolver = moduleNameResolver;

    public void Add(Type handlerType, Type contextType)
    {
        var typeFullName = handlerType.FullName!;
        var moduleName = _moduleNameResolver.Resolve(typeFullName);
        Registry.TryAdd(moduleName, contextType);
    }

    public bool TryGetContextType(Type handlerType, out Type? contextType)
    {
        var typeFullName = handlerType.FullName!;
        var moduleName = _moduleNameResolver.Resolve(typeFullName);
        return Registry.TryGetValue(moduleName, out contextType);
    }
}