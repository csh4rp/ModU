namespace ModU.Infrastructure.Messaging.Options;

public class OutboxOptions
{
    public string GlobalQueueName { get; set; } = null!;
    public int MaxRetryAttempts { get; set; }
}