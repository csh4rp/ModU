namespace ModU.Infrastructure.Events.Domain.Entities;

public class TraceInfo
{
    private TraceInfo()
    {
    }

    public TraceInfo(Guid transactionId, Guid? userId, string traceId, string spanId)
    {
        TransactionId = transactionId;
        UserId = userId;
        TraceId = traceId;
        SpanId = spanId;
    }

    public Guid TransactionId { get; private set; }
    public Guid? UserId { get; private set; }
    public string TraceId { get; private set; }
    public string SpanId { get; private set; }
}