using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Models;

public sealed class DomainEventQueue : IDomainEventQueue
{
    private readonly DomainEventQueueLock _domainEventQueueLock;
    private readonly Queue<DomainEventSnapshot> _snapshots;

    public string Id => _domainEventQueueLock.Id;
    public bool TryDequeue(out DomainEventSnapshot? domainEventSnapshot)
    {
        if (!_domainEventQueueLock.HasExpired)
        {
            return _snapshots.TryDequeue(out domainEventSnapshot);
        }
        
        domainEventSnapshot = null;
        return false;
    }
}