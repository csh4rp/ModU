using System.Runtime.CompilerServices;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Models;

namespace ModU.Infrastructure.Events.Stores;

public class DomainQueueStore : IDomainQueueStore
{
    private readonly IDomainEventQueueLockStore _domainEventQueueLockStore;
    private readonly IDomainEventSnapshotStore _domainEventSnapshotStore;
    private readonly IClock _clock;
    public async IAsyncEnumerable<IDomainEventQueue> GetQueuesToProcessAsync([EnumeratorCancellation] CancellationToken cancellationToken = new())
    {
        var queues = await _domainEventSnapshotStore.GetQueuesToBeProcessed(cancellationToken);
        foreach (var queue in queues)
        {
            yield return new DomainEventQueue(queue, _domainEventQueueLockStore, _domainEventSnapshotStore, _clock);
        }
        
        yield break;
    }

    public Task ReleaseAsync(IDomainEventQueue queue, CancellationToken cancellationToken = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}