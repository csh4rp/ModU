using Microsoft.Extensions.Options;
using ModU.Abstract.Contexts;
using ModU.Abstract.Messaging;
using ModU.Infrastructure.Messaging.Model;
using ModU.Infrastructure.Messaging.Options;
using System.Reflection;
using System.Text.Json;
using ModU.Abstract.Time;

namespace ModU.Infrastructure.Messaging.Factories;

internal sealed class OutboxMessageFactory : IOutboxMessageFactory
{
    private readonly IAppContext _appContext;
    private readonly IClock _clock;
    private readonly IOptions<OutboxOptions> _options;

    public OutboxMessageFactory(IAppContext appContext, IClock clock, IOptions<OutboxOptions> options)
    {
        _appContext = appContext;
        _clock = clock;
        _options = options;
    }

    public OutboxMessage Create(IMessage message, Guid? transactionId)
    {
        var messageType = message.GetType();
        var attr = messageType.GetCustomAttribute<MessageAttribute>();
        var metaData = new OutboxMessageMetaData(_clock.Now(), attr?.QueueName ?? _options.Value.GlobalQueueName,
            _appContext.IdentityContext?.UserId, transactionId, _appContext.TraceContext.TraceId,
            _appContext.TraceContext.SpanId);

        var content = new OutboxMessageContent(attr?.Name ?? messageType.Name, messageType.FullName!,
            JsonSerializer.SerializeToDocument(message));

        var info = new OutboxMessageDeliveryInfo(attr?.MaxRetryAttempts ?? _options.Value.MaxRetryAttempts);

        return new OutboxMessage(Guid.NewGuid(), metaData, info, content);
    }
}