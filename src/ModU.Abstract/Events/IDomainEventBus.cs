using ModU.Abstract.Domain;

namespace ModU.Abstract.Events;

public interface IDomainEventBus
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new());
}