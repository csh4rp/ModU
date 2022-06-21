using System.Reflection;
using System.Text.Json;
using ModU.Abstract.Contexts;
using ModU.Abstract.Domain;
using ModU.Abstract.Events;
using ModU.Abstract.Events.Domain;
using ModU.Abstract.Security;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Domain.Entities;
using EventInfo = ModU.Infrastructure.Events.Domain.Entities.EventInfo;

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
        var eventGroup = GetEventGroup(aggregateRoot.Id, aggregateTypeName);
        var type = domainEvent.GetType();
        var domainEventAttribute = type.GetCustomAttribute<DomainEventAttribute>()!;

        var aggregateInfo = new AggregateInfo(aggregateRoot.Id, aggregateRoot.Version, aggregateTypeName);
        var deliveryInfo = new DeliveryInfo(domainEventAttribute.MaxRetryAttempts);
        var traceInfo = new TraceInfo(transactionId, _appContext.IdentityContext?.UserId,
            _appContext.TraceContext.TraceId, _appContext.TraceContext.SpanId);
        var eventInfo = new EventInfo(domainEvent.Id, aggregateRoot.Version, eventGroup, domainEventAttribute.Name, 
            typeof(T).FullName!, JsonSerializer.SerializeToDocument(domainEvent));

        return new DomainEventSnapshot(_clock.Now(), deliveryInfo, traceInfo, eventInfo, aggregateInfo);
    }

    private string GetEventGroup(Guid aggregateId, string aggregateType) 
        => _hasher.ComputeMD5Hash(aggregateId + aggregateType);
}