using ModU.Abstract.Domain;

namespace ModU.Infrastructure.Events.Stores;

public interface IDomainEventStore
{
    Task SaveAsync(Guid aggregateId, Type aggregateType, IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = new());
}