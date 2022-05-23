namespace ModU.Infrastructure.Events.Model;

public class DomainEventMetaData
{
    private DomainEventMetaData()
    {
    }

    public DomainEventMetaData(DateTime createdAt, Guid aggregateId, string aggregateType, string queue, Guid? userId,
        Guid? transactionId, string traceId, string spanId)
    {
        CreatedAt = createdAt;
        AggregateId = aggregateId;
        AggregateType = aggregateType;
        Queue = queue;
        UserId = userId;
        TransactionId = transactionId;
        TraceId = traceId;
        SpanId = spanId;
    }

    public DateTime CreatedAt { get; private set; }
    public Guid AggregateId { get; private set; }
    public string AggregateType { get; private set; } = null!;
    public string Queue { get; private set; } = null!;
    public Guid? UserId { get; private set; }
    public Guid? TransactionId { get; private set; }
    public string TraceId { get; private set; } = null!;
    public string SpanId { get; private set; } = null!;
}