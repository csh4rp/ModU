namespace ModU.Abstract.Messaging;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MessageAttribute : Attribute
{
    public MessageAttribute(string name) => Name = name;

    public string Name { get; }
    public string? QueueName { get; set; }
    public int MaxRetryAttempts { get; set; }
}