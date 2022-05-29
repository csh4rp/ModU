using ModU.Abstract.Domain;
using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Factories;

public interface IDomainEventFactory
{
    IDomainEvent Create(DomainEventSnapshot snapshot);
}