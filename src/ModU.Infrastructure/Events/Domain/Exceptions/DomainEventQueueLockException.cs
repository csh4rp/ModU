namespace ModU.Infrastructure.Events.Domain.Exceptions;

public class DomainEventQueueLockException : Exception
{
    public DomainEventQueueLockException(string? message) : base(message)
    {
    }
}