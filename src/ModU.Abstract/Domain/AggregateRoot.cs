namespace ModU.Abstract.Domain;

public abstract class AggregateRoot : Entity
{
    private Queue<IDomainEvent>? _domainEvents;

    protected void EnqueueEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new Queue<IDomainEvent>();
        _domainEvents.Enqueue(domainEvent);
    }

    public DomainEventCollection DequeueEvents()
    {
        if (_domainEvents is null || _domainEvents.Count == 0)
        {
            return new DomainEventCollection(ArraySegment<IDomainEvent>.Empty, Id, GetType());
        }

        var result = new IDomainEvent[_domainEvents.Count];
        var index = 0;
        while (_domainEvents.TryDequeue(out var domainEvent))
        {
            result[index] = domainEvent;
            index++;
        }

        return new DomainEventCollection(result, Id, GetType());
    }
}