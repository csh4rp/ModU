using ModU.Infrastructure.Events.Models;

namespace ModU.Infrastructure.Events.Stores;

public interface IDomainQueueStore
{
    Task<IAsyncEnumerable<IDomainEventQueue>> GetQueuesToProcessAsync(CancellationToken cancellationToken = new());
}