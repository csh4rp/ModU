using ModU.Infrastructure.Events.Domain.Models;

namespace ModU.Infrastructure.Events.Domain.Stores;

public interface IDomainQueueStore
{
    IAsyncEnumerable<IDomainEventQueue> GetQueuesToProcessAsync(CancellationToken cancellationToken = new());
}