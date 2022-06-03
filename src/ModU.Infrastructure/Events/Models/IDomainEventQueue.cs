using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Models;

public interface IDomainEventQueue : IAsyncDisposable
{
    IAsyncEnumerable<DomainEventSnapshot> DequeueAsync(CancellationToken cancellationToken = new());
}