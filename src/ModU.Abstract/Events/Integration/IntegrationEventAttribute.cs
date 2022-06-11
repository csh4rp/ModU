using ModU.Abstract.Events.Common;

namespace ModU.Abstract.Events.Integration;

public sealed class IntegrationEventAttribute : EventAttribute
{
    public IntegrationEventAttribute(string name, int maxRetryAttempts = 10) : base(name, maxRetryAttempts)
    {
    }
}