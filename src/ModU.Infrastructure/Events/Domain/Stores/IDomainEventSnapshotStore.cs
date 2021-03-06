using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Stores;

public interface IDomainEventSnapshotStore
{
    Task SaveAsync(DomainEventSnapshot snapshot, CancellationToken cancellationToken = new());
    
    Task SaveAsync(IEnumerable<DomainEventSnapshot> snapshots, CancellationToken cancellationToken = new());

    Task<List<DomainEventSnapshot>> GetUndeliveredAsync(string queue, CancellationToken cancellationToken = new());

    Task<IReadOnlySet<string>> GetQueuesToBeProcessed(CancellationToken cancellationToken = new());
}