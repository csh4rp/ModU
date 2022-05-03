namespace ModU.Abstract.Messaging.Exceptions;

public sealed class MessageContractBrokenException : Exception
{
    public MessageContractBrokenException(string message) : base(message)
    {
    }
}