using System.Text.Json;

namespace ModU.Infrastructure.Messaging;

internal sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string QueueName { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string TraceId { get; set; } = null!;
    public string SpanId { get; set; } = null!;
    public Guid TransactionId { get; set; }
    public Guid? UserId { get; set; }
    public JsonDocument Data { get; set; } = null!;
}