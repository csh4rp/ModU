using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Messaging;

internal interface IOutboxMessageFactory
{
    OutboxMessage Create(IMessage message, Guid transactionId);
    
    
}