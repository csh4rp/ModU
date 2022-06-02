using ModU.Infrastructure.Events.Models;

namespace ModU.Infrastructure.Events.Stores;

public interface IDomainQueueStore
{
    IAsyncEnumerable<IDomainEventQueue> GetQueuesToProcessAsync(CancellationToken cancellationToken = new());

    Task ReleaseAsync(IDomainEventQueue queue, CancellationToken cancellationToken = new());
}