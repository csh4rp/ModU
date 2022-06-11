using ModU.Abstract.Domain;
using ModU.Abstract.Events;
using ModU.Abstract.Events.Domain;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Factories;

public class DomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Create(DomainEventSnapshot snapshot)
    {
        return null;
    }
}