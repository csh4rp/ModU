using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Models;

public interface IDomainEventQueue : IAsyncDisposable
{
    IAsyncEnumerable<DomainEventSnapshot> DequeueAsync(CancellationToken cancellationToken = new());
}