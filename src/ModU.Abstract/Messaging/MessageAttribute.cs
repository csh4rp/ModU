namespace ModU.Abstract.Messaging;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MessageAttribute : Attribute
{
    public MessageAttribute(string name) => Name = name;

    public string Name { get; }
}