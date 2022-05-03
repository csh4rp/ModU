namespace ModU.Abstract.Messaging.Contracts;

public interface IMessageContract<TMessage> where TMessage : IMessage
{
    MessageContractValidationResult Validate(Type type);
}