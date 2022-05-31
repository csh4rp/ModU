using ModU.Abstract.Domain;
using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Models;

public interface IDomainEventQueue : IAsyncDisposable
{
    string Id { get; }
    
    IAsyncEnumerable<IDomainEvent> GetEventsAsync(CancellationToken cancellationToken = new());

    ValueTask MarkAsDeliveredAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new());

    ValueTask MarkAsFailedAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new());
}