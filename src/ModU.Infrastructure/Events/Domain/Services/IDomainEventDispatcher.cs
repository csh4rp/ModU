using ModU.Abstract.Events.Domain;

namespace ModU.Infrastructure.Events.Domain.Services;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = new())
        where TEvent : IDomainEvent;
}