namespace ModU.Infrastructure.Events.Integration.Entities;

public class IntegrationEventQueueLock
{
    private IntegrationEventQueueLock()
    {
    }
    
    public IntegrationEventQueueLock(string id, DateTime acquiredAt, DateTime expiresAt)
    {
        Id = id;
        AcquiredAt = acquiredAt;
        ExpiresAt = expiresAt;
    }

    public string Id { get; private set; } = null!;
    public int Version { get; private set; }
    public DateTime AcquiredAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RenewedAt { get; private set; }

    public void Renew(DateTime renewedAt, DateTime expiredAt)
    {
        RenewedAt = renewedAt;
        ExpiresAt = expiredAt;
    }
}