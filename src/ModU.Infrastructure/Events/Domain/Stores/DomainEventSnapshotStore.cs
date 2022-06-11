using Microsoft.EntityFrameworkCore;
using ModU.Abstract.Time;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Stores;

internal sealed class DomainEventSnapshotStore : IDomainEventSnapshotStore
{
    private readonly BaseDbContext _dbContext;
    private readonly IClock _clock;

    public DomainEventSnapshotStore(BaseDbContext dbContext, IClock clock)
    {
        _dbContext = dbContext;
        _clock = clock;
    }

    public Task SaveAsync(DomainEventSnapshot snapshot, CancellationToken cancellationToken = new())
    {
        _dbContext.Add(snapshot);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task SaveAsync(IEnumerable<DomainEventSnapshot> snapshots, CancellationToken cancellationToken = new())
    {
        _dbContext.AddRange(snapshots);
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<List<DomainEventSnapshot>> GetUndeliveredAsync(string queue, CancellationToken cancellationToken = new())
        => _dbContext.Set<DomainEventSnapshot>()
            .Where(s => s.Queue == queue && !s.DeliveredAt.HasValue && !s.FailedAt.HasValue)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    

    public async Task<IReadOnlySet<string>> GetQueuesToBeProcessed(CancellationToken cancellationToken = new())
    {
        // TODO: Change to subquery
        var now = _clock.Now();
        var locks = await _dbContext.Set<DomainEventQueueLock>()
            .Where(l => l.ExpiresAt > now)
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        var result = await _dbContext.Set<DomainEventSnapshot>()
            .Where(s => !s.DeliveredAt.HasValue && !s.FailedAt.HasValue && !locks.Contains(s.Queue))
            .Select(s => s.Queue)
            .ToListAsync(cancellationToken);

        return result.ToHashSet();
    }
}