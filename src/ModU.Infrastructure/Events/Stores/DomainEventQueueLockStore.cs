using Microsoft.EntityFrameworkCore;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Stores;

internal sealed class DomainEventQueueLockStore : IDomainEventQueueLockStore
{
    private readonly BaseDbContext _dbContext;

    public DomainEventQueueLockStore(BaseDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task SaveAsync(DomainEventQueueLock domainEventQueueLock,
        CancellationToken cancellationToken = new())
    {
        _dbContext.Add(domainEventQueueLock);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<DomainEventQueueLock?> GetAsync(string id, CancellationToken cancellationToken = new())
        => _dbContext.Set<DomainEventQueueLock>()
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    

    public Task DeleteAsync(DomainEventQueueLock domainEventQueueLock, CancellationToken cancellationToken = new())
    {
        _dbContext.Remove(domainEventQueueLock);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}