namespace ModU.Infrastructure.Commands;

internal sealed class UnitOfWorkTypeRegistry
{
    private static readonly Dictionary<Type, Type> _registry = new();

    public void Add(Type handlerType, Type contextType)
    {
        _registry.Add(handlerType, contextType);
    }

    public bool TryGetContextType(Type handlerType, out Type contextType) => _registry.TryGetValue(handlerType, out contextType);
}