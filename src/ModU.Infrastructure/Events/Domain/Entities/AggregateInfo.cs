namespace ModU.Infrastructure.Events.Domain.Entities;

public class AggregateInfo
{
    private AggregateInfo()
    {
    }
    
    public AggregateInfo(Guid aggregateId, int aggregateVersion, string aggregateType)
    {
        AggregateId = aggregateId;
        AggregateVersion = aggregateVersion;
        AggregateType = aggregateType;
    }

    public Guid AggregateId { get; private set; }
    public int AggregateVersion { get; private set; }
    public string AggregateType { get; private set; }
}