using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModU.Abstract.Events.Integration;

namespace ModU.Infrastructure.Events.Integration.Services;

internal sealed class IntegrationEventDispatcher : IIntegrationEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<IntegrationEventDispatcher> _logger;

    public IntegrationEventDispatcher(IServiceProvider serviceProvider, ILogger<IntegrationEventDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task DispatchAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = new()) where TEvent : IIntegrationEvent
    {
        _logger.LogInformation("Dispatching IntegrationEvent of type: '{integrationEventType}'.", typeof(TEvent));
        var handler = _serviceProvider.GetRequiredService<IIntegrationEventHandler<TEvent>>();
        return handler.HandleAsync(integrationEvent, cancellationToken);
    }
}