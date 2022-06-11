namespace ModU.Abstract.Events.Common;

public abstract class EventAttribute : Attribute
{
    protected EventAttribute(string name, int maxRetryAttempts = 10)
    {
        Name = name;
        MaxRetryAttempts = maxRetryAttempts;
    }

    public string Name { get; }
    public int MaxRetryAttempts { get; }
}