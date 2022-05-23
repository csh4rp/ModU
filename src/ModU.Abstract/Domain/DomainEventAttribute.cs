namespace ModU.Abstract.Domain;

public sealed class DomainEventAttribute : Attribute
{
    public DomainEventAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}