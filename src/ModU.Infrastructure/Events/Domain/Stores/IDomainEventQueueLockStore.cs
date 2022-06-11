using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Stores;

public interface IDomainEventQueueLockStore
{
    Task SaveAsync(DomainEventQueueLock domainEventQueueLock, CancellationToken cancellationToken = new());

    Task<DomainEventQueueLock?> GetAsync(string id, CancellationToken cancellationToken = new());
    
    Task DeleteAsync(DomainEventQueueLock domainEventQueueLock, CancellationToken cancellationToken = new());
}