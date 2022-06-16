using ModU.Abstract.Domain;
using ModU.Abstract.Events.Domain;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Factories;

public interface IDomainEventSnapshotFactory
{
    DomainEventSnapshot Create<T>(T domainEvent, IAggregateRoot aggregateRoot, Guid transactionId) where T : IDomainEvent;
}