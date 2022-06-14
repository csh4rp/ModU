namespace ModU.Abstract.Messaging;

public interface IMessageBus
{
    Task PublishAsync<T>(IMessage<T> message, CancellationToken cancellationToken = new());
}
