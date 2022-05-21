using System.Collections;

namespace ModU.Abstract.Domain;

public sealed class DomainEventCollection : IReadOnlyCollection<IDomainEvent>
{
    private readonly IReadOnlyCollection<IDomainEvent> _domainEvents;

    public DomainEventCollection(IReadOnlyCollection<IDomainEvent> domainEvents, Guid aggregateId, Type aggregateType)
    {
        _domainEvents = domainEvents;
        AggregateId = aggregateId;
        AggregateType = aggregateType;
    }

    public IEnumerator<IDomainEvent> GetEnumerator() => _domainEvents.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int Count => _domainEvents.Count;
    
    public Guid AggregateId { get; }
    
    public Type AggregateType { get; }
}