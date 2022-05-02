namespace ModU.Abstract.Messaging.Exceptions;

public sealed class ContractBrokenException : Exception
{
    public ContractBrokenException(string message) : base(message)
    {
    }
}