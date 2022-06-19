using System.Diagnostics;
using System.Text.Json;

namespace ModU.Infrastructure.Events.Domain.Entities;

public sealed class DomainEventSnapshot
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public AggregateInfo? AggregateInfo { get; private set; }
    public string Queue { get; init; } = null!;
    public DeliveryInfo DeliveryInfo { get; private set; }
    public TraceInfo TraceInfo { get; private set; }    
    public EventInfo EventInfo { get; private set; }
}