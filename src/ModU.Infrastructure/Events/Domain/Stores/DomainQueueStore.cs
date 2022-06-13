using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Domain.Models;
using ModU.Infrastructure.Events.Domain.Options;

namespace ModU.Infrastructure.Events.Domain.Stores;

internal sealed class DomainQueueStore : IDomainQueueStore
{
    private readonly IDomainEventQueueLockStore _domainEventQueueLockStore;
    private readonly IDomainEventSnapshotStore _domainEventSnapshotStore;
    private readonly IClock _clock;
    private readonly IOptions<DomainEventOptions> _options;

    public DomainQueueStore(IDomainEventQueueLockStore domainEventQueueLockStore, IDomainEventSnapshotStore domainEventSnapshotStore, IClock clock, IOptions<DomainEventOptions> options)
    {
        _domainEventQueueLockStore = domainEventQueueLockStore;
        _domainEventSnapshotStore = domainEventSnapshotStore;
        _clock = clock;
        _options = options;
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
                
                yield return new DomainEventQueue(queue, _domainEventQueueLockStore, _domainEventSnapshotStore, _clock, _options.Value);
            }

            queues = await _domainEventSnapshotStore.GetQueuesToBeProcessed(cancellationToken);
        }
    }
}