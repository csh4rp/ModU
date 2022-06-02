using ModU.Infrastructure.Events.Models;

namespace ModU.Infrastructure.Events.Stores;

public class DomainQueueStore : IDomainQueueStore
{
    private readonly IDomainEventQueueLockStore _domainEventQueueLockStore;

    public async IAsyncEnumerable<IDomainEventQueue> GetQueuesToProcessAsync(CancellationToken cancellationToken = new())
    {
        yield break;
    }

    public Task ReleaseAsync(IDomainEventQueue queue, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}