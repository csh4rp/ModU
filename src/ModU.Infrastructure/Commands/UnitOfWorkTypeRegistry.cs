namespace ModU.Infrastructure.Commands;

internal sealed class UnitOfWorkTypeRegistry
{
    private static readonly Dictionary<Type, Type> Registry = new();

    public void Add(Type handlerType, Type contextType) => Registry.Add(handlerType, contextType);
    
    public bool TryGetContextType(Type handlerType, out Type? contextType) => Registry.TryGetValue(handlerType, out contextType);
}