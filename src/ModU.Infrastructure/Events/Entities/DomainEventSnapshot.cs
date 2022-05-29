namespace ModU.Infrastructure.Events.Entities;

public sealed class DomainEventSnapshot
{
    private DomainEventSnapshot()
    {
    }
    
    public DomainEventSnapshot(Guid id, DomainEventMetaData metaData, DomainEventDeliveryInfo deliveryInfo, DomainEventContent content)
    {
        Id = id;
        MetaData = metaData;
        DeliveryInfo = deliveryInfo;
        Content = content;
    }

    public Guid Id { get; private set; }
    public DomainEventMetaData MetaData { get; private set; } = null!;
    public DomainEventDeliveryInfo DeliveryInfo { get; private set; } = null!;
    public DomainEventContent Content { get; private set; } = null!;

    public void MarkAsDelivered(DateTime deliveredAt)
    {

    }
}