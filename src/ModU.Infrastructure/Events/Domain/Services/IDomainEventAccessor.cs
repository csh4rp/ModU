using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Services;

public interface IDomainEventAccessor
{
    IEnumerable<DomainEventSnapshot> GetEvents();
}