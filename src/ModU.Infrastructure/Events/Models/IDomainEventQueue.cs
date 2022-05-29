using ModU.Abstract.Domain;
using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Models;

public interface IDomainEventQueue : IAsyncDisposable
{
    string Id { get; }
    
    Task<bool> TryAcquireLockAsync(CancellationToken cancellationToken = new());
    
    IAsyncEnumerable<IDomainEvent> GetEventsAsync(CancellationToken cancellationToken = new());

    Task MarkAsPublishedAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new());

    Task MarkAsFailedAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new());
}