namespace ModU.Infrastructure.Events.Domain.Entities;

public class DeliveryInfo
{
    private DeliveryInfo()
    {
    }

    public DeliveryInfo(int maxAttempts) => MaxAttempts = maxAttempts;

    public int MaxAttempts { get; private set; }
    public int FailedAttempts { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? FailedAt { get; private set; }
    public DateTime? NextAttemptAt { get; private set; }
}