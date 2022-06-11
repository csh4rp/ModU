using ModU.Abstract.Domain;
using ModU.Abstract.Events;
using ModU.Abstract.Events.Domain;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Factories;

public interface IDomainEventSnapshotFactory
{
    DomainEventSnapshot Create<T>(T domainEvent, Guid aggregateId, Type aggregateType, Guid? transactionId) where T : IDomainEvent;
}