using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Stores;

public interface IDomainEventSnapshotStore
{
    Task SaveAsync(DomainEventSnapshot snapshot, CancellationToken cancellationToken = new());
    
    Task SaveAsync(IEnumerable<DomainEventSnapshot> snapshots, CancellationToken cancellationToken = new());

    Task<List<DomainEventSnapshot>> GetUndeliveredAsync(string queue, CancellationToken cancellationToken = new());

    Task<List<string>> GetQueuesToBeProcessed(CancellationToken cancellationToken = new());
}