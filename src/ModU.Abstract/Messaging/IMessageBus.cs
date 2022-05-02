namespace ModU.Abstract.Messaging;

public interface IMessageBus
{
    Task PublishAsync(IMessage message, CancellationToken cancellationToken = new());
    
    Task PublishAsync(IEnumerable<IMessage> messages, CancellationToken cancellationToken = new());
}