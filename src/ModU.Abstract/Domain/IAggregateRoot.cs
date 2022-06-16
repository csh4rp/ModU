using ModU.Abstract.Events.Domain;

namespace ModU.Abstract.Domain;

public interface IAggregateRoot
{
    Guid Id { get; }
    int Version { get; }
    IEnumerable<IDomainEvent> DequeueEvents();
}