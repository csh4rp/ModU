namespace ModU.Infrastructure.Messaging.Model;

internal sealed class OutboxMessage
{
    private OutboxMessage()
    {
    }

    public OutboxMessage(Guid id, OutboxMessageMetaData metaData, OutboxMessageDeliveryInfo deliveryInfo, OutboxMessageContent content)
    {
        Id = id;
        MetaData = metaData;
        DeliveryInfo = deliveryInfo;
        Content = content;
    }

    public Guid Id { get; private set; }
    public OutboxMessageMetaData MetaData { get; private set; } = null!;
    public OutboxMessageDeliveryInfo DeliveryInfo { get; private set; } = null!;
    public OutboxMessageContent Content { get; private set; } = null!;
}