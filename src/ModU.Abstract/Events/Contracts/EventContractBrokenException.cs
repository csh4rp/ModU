namespace ModU.Abstract.Events.Contracts;

public sealed class EventContractBrokenException : Exception
{
    public EventContractBrokenException(string message) : base(message)
    {
    }
}