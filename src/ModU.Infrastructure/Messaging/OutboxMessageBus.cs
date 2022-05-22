using ModU.Abstract.Messaging;
using ModU.Infrastructure.Messaging.Stores;

namespace ModU.Infrastructure.Messaging;

internal sealed class OutboxMessageBus : IMessageBus
{
    private readonly IOutboxMessageStore _outboxMessageStore;

    public OutboxMessageBus(IOutboxMessageStore outboxMessageStore) => _outboxMessageStore = outboxMessageStore;

    public Task PublishAsync(IMessage message, CancellationToken cancellationToken = new())
        => _outboxMessageStore.SaveAsync(message, cancellationToken);
    
    public Task PublishAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = new())
        => _outboxMessageStore.SaveAsync(messages, cancellationToken);

}