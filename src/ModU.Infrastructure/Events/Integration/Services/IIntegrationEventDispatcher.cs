using ModU.Abstract.Events.Integration;

namespace ModU.Infrastructure.Events.Integration.Services;

public interface IIntegrationEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = new()) where TEvent : IIntegrationEvent;
}