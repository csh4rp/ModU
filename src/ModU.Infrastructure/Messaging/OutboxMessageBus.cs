using ModU.Abstract.Contexts;
using ModU.Abstract.Messaging;
using ModU.Abstract.Time;
using ModU.Infrastructure.Messaging.Model;
using System.Reflection;
using ModU.Infrastructure.Modules;

namespace ModU.Infrastructure.Messaging;

internal sealed class OutboxMessageBus : IMessageBus
{
    private readonly ModuleServiceProvider _moduleServiceProvider;
    private readonly IClock _clock;
    private readonly IAppContext _appContext;

    public OutboxMessageBus(ModuleServiceProvider moduleServiceProvider, IClock clock, IAppContext appContext)
    {
        _moduleServiceProvider = moduleServiceProvider;
        _clock = clock;
        _appContext = appContext;
    }

    public async Task PublishAsync(IMessage message, CancellationToken cancellationToken = new())
    {
        var context = _moduleServiceProvider.GetDbContextForType(message.GetType());
        var transactionId = context.Database.CurrentTransaction?.TransactionId;
        var queueName = message.GetType().GetCustomAttribute<MessageAttribute>()?.QueueName ?? "global";
        var content = OutboxMessageContentFactory.Create(message);
        var metaData = new OutboxMessageMetaData
        {
            CreatedAt = _clock.Now(),
            TraceId = _appContext.TraceContext.TraceId,
            SpanId = _appContext.TraceContext.SpanId,
            UserId = _appContext.IdentityContext?.UserId,
            TransactionId = transactionId
        };

        var outboxMessage = new OutboxMessage(Guid.NewGuid(), queueName, metaData, content);
        context.Add(outboxMessage);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public Task PublishAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = new())
    {
        var messagesCollection = messages as IReadOnlyCollection<IMessage> ?? messages.ToList();
        var context = _moduleServiceProvider.GetDbContextForType(messagesCollection.First().GetType());
        var transactionId = context.Database.CurrentTransaction?.TransactionId;
        foreach (var message in messagesCollection)
        {
            var queueName = message.GetType().GetCustomAttribute<MessageAttribute>()?.QueueName ?? "global";
            var content = OutboxMessageContentFactory.Create(message);
            var metaData = new OutboxMessageMetaData
            {
                CreatedAt = _clock.Now(),
                TraceId = _appContext.TraceContext.TraceId,
                SpanId = _appContext.TraceContext.SpanId,
                UserId = _appContext.IdentityContext?.UserId,
                TransactionId = transactionId
            };

            var outboxMessage = new OutboxMessage(Guid.NewGuid(), queueName, metaData, content);
            context.Add(outboxMessage);
        }
        
        return context.SaveChangesAsync(cancellationToken);
    }
}