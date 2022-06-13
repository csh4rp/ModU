using ModU.Infrastructure.Events.Integration.Entities;

namespace ModU.Infrastructure.Events.Integration.Factories;

public interface IIntegrationEventSnapshotFactory
{
    IntegrationEventSnapshot Create<T>(T integrationEvent, Guid transactionId, string? queue = null);
}