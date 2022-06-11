namespace ModU.Abstract.Events.Contracts;

internal sealed class PropertyContract
{
    public PropertyContract(string propertyName, Type propertyType, bool ignore)
    {
        PropertyName = propertyName;
        PropertyType = propertyType;
        Ignore = ignore;
    }

    public string PropertyName { get; }
    public Type PropertyType { get; }
    public bool Ignore { get; }
}