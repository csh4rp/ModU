namespace ModU.Infrastructure.Events.Domain.Entities;

public class TraceInfo
{
    public Guid TransactionId { get; private set; }
    public Guid? UserId { get; private set; }
    public string TraceId { get; private set; }
    public string SpanId { get; private set; }
}