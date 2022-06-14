using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModU.Abstract.Events.Domain;

namespace ModU.Infrastructure.Events.Domain.Services;

internal sealed class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Task DispatchAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = new())
        where TEvent : IDomainEvent
    {
        _logger.LogInformation("Dispatching DomainEvent of type: '{domainEventType}'.", typeof(TEvent));
        var handler = _serviceProvider.GetRequiredService<IDomainEventHandler<TEvent>>();
        return handler.HandleAsync(domainEvent, cancellationToken);
    }
}