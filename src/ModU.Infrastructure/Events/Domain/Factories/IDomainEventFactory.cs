using ModU.Abstract.Events.Domain;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Factories;

public interface IDomainEventFactory
{
    IDomainEvent Create(DomainEventSnapshot snapshot);
}