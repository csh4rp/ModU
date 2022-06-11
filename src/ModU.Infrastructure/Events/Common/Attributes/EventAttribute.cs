namespace ModU.Infrastructure.Events.Common.Attributes;

public class EventAttribute : Attribute
{
    public EventAttribute(string name, int maxRetryAttempts = 10)
    {
        Name = name;
        MaxRetryAttempts = maxRetryAttempts;
    }

    public string Name { get; }
    public int MaxRetryAttempts { get; }
}