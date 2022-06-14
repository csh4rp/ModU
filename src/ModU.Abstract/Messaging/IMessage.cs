namespace ModU.Abstract.Messaging;

public interface IMessage<out TPayload>
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
    TPayload Payload { get; }
}