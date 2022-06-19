using ModU.Abstract.Events.Domain;
using ModU.Abstract.Events.Integration;

namespace ModU.Infrastructure.Events.Common.Mapping;

public interface IEventMapper
{
    IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> domainEvents);
}