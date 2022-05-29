namespace ModU.Infrastructure.Events.Entities;

public class DomainEventQueueLock
{
    public DomainEventQueueLock()
    {
    }
    
    public DomainEventQueueLock(string id, DateTime acquiredAt, DateTime expiresAt)
    {
        Id = id;
        AcquiredAt = acquiredAt;
        ExpiresAt = expiresAt;
    }

    public string Id { get; private set; } = null!;
    public DateTime AcquiredAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RenewedAt { get; private set; }

    public void Renew(DateTime renewedAt, DateTime expiredAt)
    {
        RenewedAt = renewedAt;
        ExpiresAt = expiredAt;
    }
}