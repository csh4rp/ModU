namespace ModU.Infrastructure.Messaging.Model;

internal enum OutboxMessageType
{
    DomainEvent = 1,
    Message = 2
}