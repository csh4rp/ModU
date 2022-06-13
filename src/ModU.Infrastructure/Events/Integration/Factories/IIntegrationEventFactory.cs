using ModU.Abstract.Events.Integration;
using ModU.Infrastructure.Events.Integration.Entities;

namespace ModU.Infrastructure.Events.Integration.Factories;

public interface IIntegrationEventFactory
{
    IEnumerable<IIntegrationEvent> Create(IntegrationEventSnapshot snapshot);
}