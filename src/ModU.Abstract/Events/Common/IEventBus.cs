namespace ModU.Abstract.Events.Common;

public interface IEventBus
{
    Task PublishAsync(IEvent @event, CancellationToken cancellationToken = new());
    
    Task PublishAsync(IEvent @event, string queue, CancellationToken cancellationToken = new());
}