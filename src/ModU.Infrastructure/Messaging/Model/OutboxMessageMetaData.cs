namespace ModU.Infrastructure.Messaging.Model;

public class OutboxMessageMetaData
{
    public DateTime CreatedAt { get; init; }
    public Guid? UserId { get; init; }
    public Guid? TransactionId { get; init; }
    public string TraceId { get; init; } = null!;
    public string SpanId { get; init; } = null!;
}