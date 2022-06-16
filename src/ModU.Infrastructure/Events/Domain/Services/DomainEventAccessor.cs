using ModU.Abstract.Domain;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.Events.Domain.Entities;
using ModU.Infrastructure.Events.Domain.Factories;

namespace ModU.Infrastructure.Events.Domain.Services;

internal sealed class DomainEventAccessor : IDomainEventAccessor
{
    private static readonly AsyncLocal<List<DomainEventSnapshot>> EventsAccessor = new();
    private readonly BaseDbContext _dbContext;
    private readonly IDomainEventSnapshotFactory _domainEventSnapshotFactory;

    public DomainEventAccessor(BaseDbContext dbContext, IDomainEventSnapshotFactory domainEventSnapshotFactory)
    {
        _dbContext = dbContext;
        _domainEventSnapshotFactory = domainEventSnapshotFactory;
    }

    public IEnumerable<DomainEventSnapshot> GetEvents()
    {
        var transactionId = _dbContext.Database.CurrentTransaction?.TransactionId ?? Guid.NewGuid();
        var aggregates = _dbContext.ChangeTracker.Entries().Where(e => e.Entity is IAggregateRoot)
            .Select(e => (IAggregateRoot) e.Entity);

        EventsAccessor.Value ??= new List<DomainEventSnapshot>();
        foreach (var aggregateRoot in aggregates)
        {
            var events = aggregateRoot.DequeueEvents();
            var snapshots = events.Select(e => _domainEventSnapshotFactory.Create(e, aggregateRoot, transactionId));
            EventsAccessor.Value.AddRange(snapshots);
        }

        return EventsAccessor.Value;
    }
}