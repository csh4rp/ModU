namespace ModU.Abstract.Messaging;

public interface IMessage<out TPayload>
{
    Guid Id { get; }
    long SequenceNumber { get; }
    DateTime CreatedAt { get; }
    TPayload Payload { get; }
}