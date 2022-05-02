namespace ModU.Abstract.Messaging;

public interface IMessageContract<TMessage> where TMessage : IMessage
{
    void Validate(Type type);
}