using ModU.Abstract.Events;
using ModU.Abstract.Events.Domain;

namespace ModU.Abstract.Domain;

public abstract class AggregateRoot : Entity
{
    private Queue<IDomainEvent>? _domainEvents;

    protected void EnqueueEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new Queue<IDomainEvent>();
        _domainEvents.Enqueue(domainEvent);
    }

    public IEnumerable<IDomainEvent> DequeueEvents()
    {
        if (_domainEvents is null || _domainEvents.Count == 0)
        {
            yield break;
        }
        
        while (_domainEvents.TryDequeue(out var domainEvent))
        {
            yield return domainEvent;
        }
    }
}