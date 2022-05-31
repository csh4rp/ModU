using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Stores;

public interface IDomainEventSnapshotStore
{
    Task SaveAsync(DomainEventSnapshot snapshot, CancellationToken cancellationToken = new());
    
    Task SaveAsync(IEnumerable<DomainEventSnapshot> snapshots, CancellationToken cancellationToken = new());

    Task<List<DomainEventSnapshot>> GetEventsBatchAsync(string queue, int batchIndex, CancellationToken cancellationToken = new());
}