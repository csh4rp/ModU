using ModU.Abstract.Domain;
using ModU.Infrastructure.Events.Model;

namespace ModU.Infrastructure.Events.Factories;

public interface IDomainEventSnapshotFactory
{
    DomainEventSnapshot Create<T>(T domainEvent, Guid aggregateId, Type aggregateType, Guid? transactionId) where T : IDomainEvent;
}