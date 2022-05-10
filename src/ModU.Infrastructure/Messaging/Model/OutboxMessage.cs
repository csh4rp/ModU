namespace ModU.Infrastructure.Messaging.Model;

internal sealed class OutboxMessage
{
    private OutboxMessage()
    {
    }

    public OutboxMessage(Guid id, string queueName, OutboxMessageMetaData metaData, OutboxMessageContent content)
    {
        Id = id;
        QueueName = queueName;
        MetaData = metaData;
        Content = content;
    }

    public Guid Id { get; private set; }
    public OutboxMessageType Type { get; private set; }
    public string QueueName { get; private set; } = null!;
    public OutboxMessageMetaData MetaData { get; private set; } = null!;
    public OutboxMessageContent Content { get; private set; } = null!;
}