using ModU.Abstract.Contexts;
using ModU.Abstract.Messaging;
using ModU.Abstract.Time;
using ModU.Infrastructure.Messaging.Model;
using System.Reflection;

namespace ModU.Infrastructure.Messaging;

internal sealed class OutboxMessageBus : IMessageBus
{
    private readonly IAppContext _appContext;
    private readonly ITraceContext _traceContext;
    private readonly IClock _clock;

    
    public Task PublishAsync(IMessage message, CancellationToken cancellationToken = new())
    {
        var metaData = GetMetaData();
        var content = OutboxMessageContentFactory.Create(message);
        var queueName = GetQueueName(message);
        var outboxMessage = new OutboxMessage(Guid.NewGuid(), queueName, metaData, content);
        return Task.CompletedTask;
    }

    public Task PublishAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = new())
    {
        throw new NotImplementedException();
    }

    private OutboxMessageMetaData GetMetaData() =>
        new()
        {
            CreatedAt = _clock.Now(),
            UserId = _appContext.IdentityContext?.UserId,
            TransactionId = null,
            TraceId = _traceContext.TraceId,
            SpanId = _traceContext.SpanId
        };

    private static string GetQueueName(IMessage message)
    {
        var messageType = message.GetType();
        var messageAttribute = messageType.GetCustomAttribute<MessageAttribute>();
        return messageAttribute is null ? messageType.FullName! : messageAttribute.QueueName;
    }
}