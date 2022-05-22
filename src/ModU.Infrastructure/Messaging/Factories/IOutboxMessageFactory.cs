using ModU.Abstract.Messaging;
using ModU.Infrastructure.Messaging.Model;

namespace ModU.Infrastructure.Messaging.Factories;

internal interface IOutboxMessageFactory
{
    OutboxMessage Create(IMessage message, Guid? transactionId);
}