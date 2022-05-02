using ModU.Abstract.Events;

namespace ModU.Abstract.Domain;

public abstract class AggregateRoot : Entity
{
    private List<IDomainEvent>? _domainEvents;

    public IReadOnlyCollection<IDomainEvent> DomainEvents
    {
        get
        {
            if (_domainEvents is null)
            {
                return Array.Empty<IDomainEvent>();
            }

            return _domainEvents;
        }
    }

    protected void AddEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(domainEvent);
    }

    public void ClearEvents() => _domainEvents?.Clear();
}