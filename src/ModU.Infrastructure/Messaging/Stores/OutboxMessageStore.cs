using ModU.Abstract.Messaging;
using ModU.Infrastructure.Messaging.Factories;
using ModU.Infrastructure.Modules;

namespace ModU.Infrastructure.Messaging.Stores;

internal sealed class OutboxMessageStore : IOutboxMessageStore
{
    private readonly IModuleServiceProvider _moduleServiceProvider;
    private readonly IOutboxMessageFactory _outboxMessageFactory;

    public OutboxMessageStore(IModuleServiceProvider moduleServiceProvider, IOutboxMessageFactory outboxMessageFactory)
    {
        _moduleServiceProvider = moduleServiceProvider;
        _outboxMessageFactory = outboxMessageFactory;
    }

    public Task SaveAsync(IMessage message, CancellationToken cancellationToken = new())
    {
        var context = _moduleServiceProvider.GetDbContextForType(message.GetType());
        var outboxMessage = _outboxMessageFactory.Create(message, context.Database.CurrentTransaction?.TransactionId);

        context.Add(outboxMessage);
        return context.SaveChangesAsync(cancellationToken);
    }

    public Task SaveAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = new())
    {
        var messagesCollection = messages as IReadOnlyCollection<IMessage> ?? messages.ToList();
        var context = _moduleServiceProvider.GetDbContextForType(messagesCollection.First().GetType());
        var transactionId = context.Database.CurrentTransaction?.TransactionId;
        var outboxMessages = messagesCollection.Select(m => _outboxMessageFactory.Create(m, transactionId));

        context.AddRange(outboxMessages);
        return context.SaveChangesAsync(cancellationToken);
    }
}