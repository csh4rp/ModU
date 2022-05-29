using ModU.Abstract.Domain;
using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Factories;

public class DomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Create(DomainEventSnapshot snapshot)
    {
        return null;
    }
}