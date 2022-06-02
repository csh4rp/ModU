using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Stores;

public interface IDomainEventQueueLockStore
{
    Task SaveAsync(DomainEventQueueLock domainEventQueueLock, CancellationToken cancellationToken = new());

    Task DeleteAsync(DomainEventQueueLock domainEventQueueLock, CancellationToken cancellationToken = new());
}