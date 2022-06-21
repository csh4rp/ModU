using System.Diagnostics;
using System.Text.Json;

namespace ModU.Infrastructure.Events.Domain.Entities;

public sealed class DomainEventSnapshot
{
    private DomainEventSnapshot()
    {
    }

    public DomainEventSnapshot(DateTime createdAt, DeliveryInfo deliveryInfo, TraceInfo traceInfo, EventInfo eventInfo,
        AggregateInfo aggregateInfo)
    {
        CreatedAt = createdAt;
        DeliveryInfo = deliveryInfo;
        TraceInfo = traceInfo;
        EventInfo = eventInfo;
        AggregateInfo = aggregateInfo;
    }

    public long Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public AggregateInfo? AggregateInfo { get; private set; }
    public DeliveryInfo DeliveryInfo { get; private set; } = null!;
    public TraceInfo TraceInfo { get; private set; } = null!;
    public EventInfo EventInfo { get; private set; } = null!;
}