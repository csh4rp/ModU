namespace ModU.Infrastructure.Messaging.Model;

public class OutboxMessageMetaData
{
    private OutboxMessageMetaData()
    {
    }
    
    public OutboxMessageMetaData(DateTime createdAt, string queue, Guid? userId, Guid? transactionId, string traceId, string spanId)
    {
        CreatedAt = createdAt;
        Queue = queue;
        UserId = userId;
        TransactionId = transactionId;
        TraceId = traceId;
        SpanId = spanId;
    }

    public DateTime CreatedAt { get; private set; }
    public string Queue { get; private set; } = null!;
    public Guid? UserId { get; private set; }
    public Guid? TransactionId { get; private set; }
    public string TraceId { get; private set; } = null!;
    public string SpanId { get; private set; } = null!;
}