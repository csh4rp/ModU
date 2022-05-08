using System.Reflection;
using System.Text.Json;
using ModU.Abstract.Contexts;
using ModU.Abstract.Messaging;
using ModU.Abstract.Time;

namespace ModU.Infrastructure.Messaging;

internal sealed class OutboxMessageFactory
{
    private readonly IAppContext _appContext;
    private readonly IClock _clock;

    public OutboxMessageFactory(IAppContext appContext, IClock clock)
    {
        _appContext = appContext;
        _clock = clock;
    }

    public OutboxMessage Create(IMessage message, Guid transactionId)
    {
        var messageType = message.GetType();
        var (messageName, queueName) = GetMetaData(messageType);
        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            CreatedAt = _clock.Now(),
            Name = messageName,
            Type = messageType.FullName!,
            QueueName = queueName,
            TraceId = _appContext.TraceContext.TraceId,
            SpanId = _appContext.TraceContext.SpanId,
            TransactionId = transactionId,
            UserId = _appContext.IdentityContext?.UserId,
            Data = GetData(message)
        };
    }

    private static (string messageName, string queueName) GetMetaData(Type messageType)
    {
        const string defaultQueueName = "global";
        var attribute = messageType.GetCustomAttribute<MessageAttribute>();
        return attribute is null
            ? (messageType.FullName!, defaultQueueName)
            : (attribute.Name, attribute.QueueName ?? defaultQueueName);
    }
    
    private static JsonDocument GetData(IMessage message)
        => JsonSerializer.SerializeToDocument(message);
}