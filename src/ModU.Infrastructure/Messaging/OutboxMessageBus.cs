using ModU.Abstract.Messaging;

namespace ModU.Infrastructure.Messaging;

internal sealed class OutboxMessageBus : IMessageBus
{
    
    
    public Task PublishAsync(IMessage message, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public Task PublishAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}