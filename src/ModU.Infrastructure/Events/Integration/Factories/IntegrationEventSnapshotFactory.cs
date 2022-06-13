using System.Reflection;
using System.Text.Json;
using ModU.Abstract.Contexts;
using ModU.Abstract.Events.Domain;
using ModU.Abstract.Events.Integration;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Domain.Entities;
using ModU.Infrastructure.Events.Integration.Entities;

namespace ModU.Infrastructure.Events.Integration.Factories;

internal sealed class IntegrationEventSnapshotFactory : IIntegrationEventSnapshotFactory
{
    private readonly IClock _clock;
    private readonly IAppContext _appContext;
    
    
    public IntegrationEventSnapshotFactory(IClock clock, IAppContext appContext)
    {
        _clock = clock;
        _appContext = appContext;
    }

    public IntegrationEventSnapshot Create<T>(T integrationEvent, Guid transactionId, string? queue = null)
    {
        var type = typeof(T);
        var integrationEventAttribute = type.GetCustomAttribute<IntegrationEventAttribute>();
        var queueName = queue ?? integrationEventAttribute?.Queue;
        if (string.IsNullOrEmpty(queue))
        {
            throw new InvalidOperationException("A queue has to be specified.");
        }

        return new IntegrationEventSnapshot
        {
            Id = Guid.NewGuid(),
            CreatedAt = _clock.Now(),
            Queue = queueName,
            UserId = _appContext.IdentityContext?.UserId,
            TransactionId = transactionId,
            TraceId = _appContext.TraceContext.TraceId,
            SpanId = _appContext.TraceContext.SpanId,
            MaxAttempts = 10,
            Name = integrationEventAttribute?.Name ?? type.Name,
            Type = type.FullName!,
            Data = JsonSerializer.SerializeToDocument(integrationEvent)
        };
    }
}