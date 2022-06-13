using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using ModU.Abstract.Time;
using ModU.Infrastructure.Events.Domain.Entities;
using ModU.Infrastructure.Events.Domain.Exceptions;
using ModU.Infrastructure.Events.Domain.Options;
using ModU.Infrastructure.Events.Domain.Stores;

namespace ModU.Infrastructure.Events.Domain.Models;

internal sealed class DomainEventQueue : IDomainEventQueue
{
    private readonly string _id;
    private readonly IDomainEventQueueLockStore _queueLockStore;
    private readonly IDomainEventSnapshotStore _domainEventSnapshotStore;
    private readonly IClock _clock;
    private readonly DomainEventOptions _options;
    private DomainEventQueueLock? _lock;

    public DomainEventQueue(string id, IDomainEventQueueLockStore queueLockStore, IDomainEventSnapshotStore domainEventSnapshotStore, 
        IClock clock, DomainEventOptions options)
    {
        _id = id;
        _queueLockStore = queueLockStore;
        _domainEventSnapshotStore = domainEventSnapshotStore;
        _clock = clock;
        _options = options;
    }

    public async IAsyncEnumerable<DomainEventSnapshot> DequeueAsync([EnumeratorCancellation] CancellationToken cancellationToken = new())
    {
        if (!await TryAcquireLockAsync(cancellationToken))
        {
            yield break;
        }

        var events = await _domainEventSnapshotStore.GetUndeliveredAsync(_id, cancellationToken);
        while (events.Any() && !cancellationToken.IsCancellationRequested)
        {
            foreach (var snapshot in events)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }
                
                if (_lock!.ExpiresAt < _clock.Now() && !await TryRenewLockAsync(cancellationToken))
                {
                    yield break;
                }
                
                yield return snapshot;
            }

            events = await _domainEventSnapshotStore.GetUndeliveredAsync(_id, cancellationToken);
        }
    }

    private async Task<bool> TryAcquireLockAsync(CancellationToken cancellationToken)
    {
        _lock = await _queueLockStore.GetAsync(_id, cancellationToken);
        if (_lock?.ExpiresAt > _clock.Now())
        {
            return false;
        }
        
        _lock = new DomainEventQueueLock(_id, _clock.Now(), _clock.Now().AddSeconds(_options.QueueLockTime));
        try
        {
            await _queueLockStore.SaveAsync(_lock, cancellationToken);
            return true;
        }
        catch (DomainEventQueueLockException)
        {
            _lock = null;
            return false;
        }
    }

    private async Task<bool> TryRenewLockAsync(CancellationToken cancellationToken)
    {
        _lock!.Renew(_clock.Now(), _clock.Now().AddSeconds(60));
        try
        {
            await _queueLockStore.SaveAsync(_lock, cancellationToken);
            return true;
        }
        catch (DomainEventQueueLockException)
        {
            return false;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_lock is null)
        {
            return;
        }

        await _queueLockStore.DeleteAsync(_lock);
    }
}