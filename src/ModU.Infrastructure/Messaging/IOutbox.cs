namespace ModU.Infrastructure.Messaging;

public interface IOutbox
{
    Task SaveAsync(object message);
}