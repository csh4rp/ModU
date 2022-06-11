using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Options;
using ModU.Abstract.Contexts;
using ModU.Abstract.Domain;
using ModU.Abstract.Security;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Domain.Entities;
using ModU.Infrastructure.Events.Domain.Options;

namespace ModU.Infrastructure.Events.Domain.Factories;

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
        var type = domainEvent.GetType();
        var domainEventAttribute = type.GetCustomAttribute<DomainEventAttribute>();

        return new DomainEventSnapshot
        {
            Id = Guid.NewGuid(),
            CreatedAt = _clock.Now(),
            AggregateId = aggregateId,
            AggregateType = aggregateTypeName,
            Queue = queueName,
            UserId = _appContext.IdentityContext?.UserId,
            TransactionId = transactionId,
            TraceId = _appContext.TraceContext.TraceId,
            SpanId = _appContext.TraceContext.SpanId,
            MaxAttempts = 10,
            Name = domainEventAttribute?.Name ?? type.Name,
            Type = type.FullName!,
            Data = JsonSerializer.SerializeToDocument(domainEvent)
        };
    }

    private string GetQueueName(Guid aggregateId, string aggregateType) 
        => _hasher.ComputeMD5Hash(aggregateId + aggregateType);
}