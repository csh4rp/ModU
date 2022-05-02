namespace ModU.Abstract.Events;

public interface IDomainEventBus
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new());
}