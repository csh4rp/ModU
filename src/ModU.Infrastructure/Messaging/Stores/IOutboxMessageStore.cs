using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Messaging.Stores;

internal interface IOutboxMessageStore
{
    Task SaveAsync(IMessage message, CancellationToken cancellationToken = new());
    
    Task SaveAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = new());
    
    
}