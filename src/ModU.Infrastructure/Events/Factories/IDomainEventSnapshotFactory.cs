using ModU.Abstract.Domain;
using ModU.Infrastructure.Events.Model;

namespace ModU.Infrastructure.Events.Factories;

public interface IDomainEventSnapshotFactory
{
    DomainEventSnapshot Create(IDomainEvent domainEvent, Guid aggregateId, Type aggregateType, Guid? transactionId);
}