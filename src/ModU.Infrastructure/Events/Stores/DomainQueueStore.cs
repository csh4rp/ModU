using System.Runtime.CompilerServices;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Models;

namespace ModU.Infrastructure.Events.Stores;

internal sealed class DomainQueueStore : IDomainQueueStore
{
    private readonly IDomainEventQueueLockStore _domainEventQueueLockStore;
    private readonly IDomainEventSnapshotStore _domainEventSnapshotStore;
    private readonly IClock _clock;

    public DomainQueueStore(IDomainEventQueueLockStore domainEventQueueLockStore, IDomainEventSnapshotStore domainEventSnapshotStore, IClock clock)
    {
        _domainEventQueueLockStore = domainEventQueueLockStore;
        _domainEventSnapshotStore = domainEventSnapshotStore;
        _clock = clock;
    }

    public async IAsyncEnumerable<IDomainEventQueue> GetQueuesToProcessAsync([EnumeratorCancellation] CancellationToken cancellationToken = new())
    {
        var queues = await _domainEventSnapshotStore.GetQueuesToBeProcessed(cancellationToken);
        while (queues.Any())
        {
            foreach (var queue in queues)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }
                
                yield return new DomainEventQueue(queue, _domainEventQueueLockStore, _domainEventSnapshotStore, _clock);
            }

            queues = await _domainEventSnapshotStore.GetQueuesToBeProcessed(cancellationToken);
        }
    }
}