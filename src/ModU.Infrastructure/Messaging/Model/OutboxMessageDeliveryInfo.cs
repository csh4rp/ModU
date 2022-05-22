namespace ModU.Infrastructure.Messaging.Model;

public class OutboxMessageDeliveryInfo
{
    public OutboxMessageDeliveryInfo(int maxAttempts)
    {
        MaxAttempts = maxAttempts;
    }

    public int AttemptNumber { get; private set; }
    public int MaxAttempts { get; private set; }
    public DateTime? NextAttemptAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? FailedAt { get; private set; }
}