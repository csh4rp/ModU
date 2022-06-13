using System.Text.Json;
using ModU.Abstract.Events.Integration;
using ModU.Infrastructure.Events.Integration.Entities;

namespace ModU.Infrastructure.Events.Integration.Factories;

internal sealed class IntegrationEventFactory : IIntegrationEventFactory
{
    public IEnumerable<IIntegrationEvent> Create(IntegrationEventSnapshot snapshot)
    {
        var types = IntegrationEventTypeContainer.GetTypes(snapshot.Name);
        foreach (var type in types)
        {
            yield return (IIntegrationEvent)snapshot.Data.Deserialize(type)!;
        }
    }
}