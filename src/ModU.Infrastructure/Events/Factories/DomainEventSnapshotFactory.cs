using Microsoft.Extensions.Options;
using ModU.Abstract.Contexts;
using ModU.Abstract.Domain;
using ModU.Abstract.Security;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Model;
using ModU.Infrastructure.Events.Options;
using System.Reflection;
using System.Text.Json;

namespace ModU.Infrastructure.Events.Factories;

internal sealed class DomainEventSnapshotFactory : IDomainEventSnapshotFactory
{
    private readonly IAppContext _appContext;
    private readonly IClock _clock;
    private readonly IHasher _hasher;
    private readonly IOptions<DomainEventOptions> _options;

    public DomainEventSnapshotFactory(IAppContext appContext, IClock clock, IHasher hasher, IOptions<DomainEventOptions> options)
    {
        _appContext = appContext;
        _clock = clock;
        _hasher = hasher;
        _options = options;
    }

    public DomainEventSnapshot Create<T>(T domainEvent, Guid aggregateId, Type aggregateType, Guid? transactionId) where T : IDomainEvent
    {
        var aggregateTypeName = aggregateType.FullName!;
        var queueName = GetQueueName(aggregateId, aggregateTypeName);
        var metaData = new DomainEventMetaData(_clock.Now(), aggregateId, aggregateTypeName, queueName,
            _appContext.IdentityContext?.UserId, transactionId,
            _appContext.TraceContext.TraceId, _appContext.TraceContext.SpanId);

        var type = domainEvent.GetType();
        var deliveryInfo = new DomainEventDeliveryInfo(_options.Value.MaxRetryAttempts);
        var domainEventAttribute = type.GetCustomAttribute<DomainEventAttribute>();
        var content = new DomainEventContent(domainEventAttribute?.Name ?? type.Name, type.FullName!,
            JsonSerializer.SerializeToDocument(domainEvent));

        return new DomainEventSnapshot(Guid.NewGuid(), metaData, deliveryInfo, content);
    }

    private string GetQueueName(Guid aggregateId, string aggregateType) 
        => _hasher.ComputeMD5Hash(aggregateId + aggregateType);
}