using System.Text.Json;

namespace ModU.Infrastructure.Events.Domain.Entities;

public class EventInfo
{
    private EventInfo()
    {
    }

    public EventInfo(Guid eventId, long sequenceNumber, string eventGroup, string eventName, string eventType, JsonDocument eventData)
    {
        EventId = eventId;
        SequenceNumber = sequenceNumber;
        EventGroup = eventGroup;
        EventName = eventName;
        EventType = eventType;
        EventData = eventData;
    }

    public Guid EventId { get; private set; }
    public long SequenceNumber { get; private set; }
    public string EventGroup { get; private set; } = null!;
    public string EventName { get; private set; } = null!;
    public string EventType { get; private set; } = null!;
    public JsonDocument EventData { get; private set; } = null!;
}