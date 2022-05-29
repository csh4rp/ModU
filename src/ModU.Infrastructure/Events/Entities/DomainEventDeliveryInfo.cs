namespace ModU.Infrastructure.Events.Entities;

public class DomainEventDeliveryInfo
{
    private DomainEventDeliveryInfo()
    {
    }

    public DomainEventDeliveryInfo(int maxAttempts) => MaxAttempts = maxAttempts;

    public int AttemptNumber { get; private set; }
    public int MaxAttempts { get; private set; }
    public DateTime? NextAttemptAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? FailedAt { get; private set; }
}