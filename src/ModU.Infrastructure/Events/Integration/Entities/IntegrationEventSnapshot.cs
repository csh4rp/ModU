using System.Text.Json;

namespace ModU.Infrastructure.Events.Integration.Entities;

public class IntegrationEventSnapshot
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }
    public string Queue { get; init; } = null!;
    public Guid? UserId { get; init; }
    public Guid? TransactionId { get; init; }
    public string TraceId { get; init; } = null!;
    public string SpanId { get; init; } = null!;
    public int FailedAttempts { get; private set; }
    public int MaxAttempts { get; init; }
    public DateTime? NextAttemptAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? FailedAt { get; private set; }
    public string Name { get; init; } = null!;
    public string Type { get; init; } = null!;
    public JsonDocument Data { get; init; }
}