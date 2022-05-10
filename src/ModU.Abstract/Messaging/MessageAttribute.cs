namespace ModU.Abstract.Messaging;

[AttributeUsage(AttributeTargets.Class)]
public sealed class MessageAttribute : Attribute
{
    public MessageAttribute(string name, string queueName)
    {
        Name = name;
        QueueName = queueName;
    }

    public string Name { get; }
    public string QueueName { get; }
}