using ModU.Abstract.Events.Domain;

namespace ModU.Abstract.Domain;

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private readonly Queue<IDomainEvent> _domainEvents = new(1);
    private int _version;

    protected void EnqueueEvent(IDomainEvent domainEvent) => _domainEvents.Enqueue(domainEvent);
    
    IEnumerable<IDomainEvent> IAggregateRoot.DequeueEvents()
    {
        if (_domainEvents.Count == 0)
        {
            yield break;
        }
        
        while (_domainEvents.TryDequeue(out var domainEvent))
        {
            yield return domainEvent;
        }
    }

    int IAggregateRoot.Version => _version;
}