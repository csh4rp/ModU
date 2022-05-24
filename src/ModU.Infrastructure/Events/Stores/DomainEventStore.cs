using ModU.Abstract.Domain;
using ModU.Infrastructure.Events.Factories;
using ModU.Infrastructure.Modules;

namespace ModU.Infrastructure.Events.Stores;

internal sealed class DomainEventStore : IDomainEventStore
{
    private readonly IModuleServiceProvider _serviceProvider;
    private readonly IDomainEventSnapshotFactory _domainEventSnapshotFactory;
    
    public DomainEventStore(IModuleServiceProvider serviceProvider, IDomainEventSnapshotFactory domainEventSnapshotFactory)
    {
        _serviceProvider = serviceProvider;
        _domainEventSnapshotFactory = domainEventSnapshotFactory;
    }

    public Task SaveAsync(Guid aggregateId, Type aggregateType, IEnumerable<IDomainEvent> domainEvents,
        CancellationToken cancellationToken = new())
    {
        var context = _serviceProvider.GetDbContextForType(aggregateType);
        var transactionId = context.Database.CurrentTransaction?.TransactionId;

        var snapshots = domainEvents.Select(de =>
            _domainEventSnapshotFactory.Create(de, aggregateId, aggregateType, transactionId));
        
        context.AddRange(snapshots);
        return context.SaveChangesAsync(cancellationToken);
    }
    
}