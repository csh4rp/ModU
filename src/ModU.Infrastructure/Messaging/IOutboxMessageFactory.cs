using ModU.Abstract.Domain;
using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Messaging;

internal interface IOutboxMessageFactory
{
    OutboxMessage Create(IMessage message, Guid transactionId);

    OutboxMessage Create(AggregateRoot aggregateRoot, IDomainEvent domainEvent, Guid transactionId);

}