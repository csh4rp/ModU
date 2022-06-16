using System.Reflection;
using System.Text.Json;
using ModU.Abstract.Contexts;
using ModU.Abstract.Domain;
using ModU.Abstract.Events;
using ModU.Abstract.Events.Domain;
using ModU.Abstract.Security;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Factories;

internal sealed class DomainEventSnapshotFactory : IDomainEventSnapshotFactory
{
    private readonly IAppContext _appContext;
    private readonly IClock _clock;
    private readonly IHasher _hasher;

    public DomainEventSnapshotFactory(IAppContext appContext, IClock clock, IHasher hasher)
    {
        _appContext = appContext;
        _clock = clock;
        _hasher = hasher;
    }

    public DomainEventSnapshot Create<T>(T domainEvent, IAggregateRoot aggregateRoot, Guid transactionId) where T : IDomainEvent
    {
        var aggregateTypeName = aggregateRoot.GetType().FullName!;
        var queueName = GetQueueName(aggregateRoot.Id, aggregateTypeName);
        var type = domainEvent.GetType();
        var domainEventAttribute = type.GetCustomAttribute<DomainEventAttribute>();

        return new DomainEventSnapshot
        {
            Id = Guid.NewGuid(),
            CreatedAt = _clock.Now(),
            AggregateId = aggregateRoot.Id,
            AggregateType = aggregateTypeName,
            AggregateVersion = aggregateRoot.Version,
            Queue = queueName,
            UserId = _appContext.IdentityContext?.UserId,
            TransactionId = transactionId,
            TraceId = _appContext.TraceContext.TraceId,
            SpanId = _appContext.TraceContext.SpanId,
            MaxAttempts = domainEventAttribute?.MaxRetryAttempts ?? 10,
            Name = domainEventAttribute?.Name ?? type.Name,
            Type = type.FullName!,
            Data = JsonSerializer.SerializeToDocument(domainEvent)
        };
    }

    private string GetQueueName(Guid aggregateId, string aggregateType) 
        => _hasher.ComputeMD5Hash(aggregateId + aggregateType);
}