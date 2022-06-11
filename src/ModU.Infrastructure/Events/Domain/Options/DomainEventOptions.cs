namespace ModU.Infrastructure.Events.Domain.Options;

public class DomainEventOptions
{
    public int QueueLockTime { get; set; }
    public bool UseBatching { get; set; }
}