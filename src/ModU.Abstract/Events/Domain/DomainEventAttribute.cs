using ModU.Abstract.Events.Common;

namespace ModU.Abstract.Events.Domain;

public sealed class DomainEventAttribute : EventAttribute
{
    public DomainEventAttribute(string name, int maxRetryAttempts = 10) : base(name, maxRetryAttempts)
    {
    }
}