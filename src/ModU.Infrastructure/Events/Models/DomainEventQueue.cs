using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModU.Abstract.Domain;
using ModU.Abstract.Time;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.Events.Entities;
using ModU.Infrastructure.Events.Factories;
using ModU.Infrastructure.Events.Options;

namespace ModU.Infrastructure.Events.Models;

public class DomainEventQueue : IDomainEventQueue
{
    private readonly BaseDbContext _dbContext;
    private readonly IClock _clock;
    private readonly IDomainEventFactory _domainEventFactory;
    private readonly IOptions<DomainEventOptions> _options;

    private DomainEventQueueLock? _domainEventQueueLock;
    private Dictionary<IDomainEvent, DomainEventSnapshot> _events = new();


    public ValueTask DisposeAsync()
    {
        return new ValueTask();
    }

    public string Id { get; }

    public async Task<bool> TryAcquireLockAsync(CancellationToken cancellationToken = new())
    {
        var now = _clock.Now();
        var expiresAt = now.AddMilliseconds(_options.Value.QueueLockTime);
        _domainEventQueueLock = await _dbContext.Set<DomainEventQueueLock>().FirstOrDefaultAsync(l => l.Id == Id, cancellationToken);
        if (_domainEventQueueLock is null)
        {
            _domainEventQueueLock = new DomainEventQueueLock(Id, _clock.Now(), expiresAt);
            _dbContext.Set<DomainEventQueueLock>().Add(_domainEventQueueLock);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        if (_domainEventQueueLock.ExpiresAt > now)
        {
            return false;
        }
        
        _domainEventQueueLock.Renew(now, expiresAt);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async IAsyncEnumerable<IDomainEvent> GetEventsAsync([EnumeratorCancellation] CancellationToken cancellationToken = new())
    {
        if (_domainEventQueueLock is null)
        {
            throw new InvalidOperationException("A lock has to be acquired before events can be fetched.");
        }
        
        var batchNumber = 0;
        var events = await GetBatchAsync(batchNumber, cancellationToken);
        while (events.Any() && _domainEventQueueLock.ExpiresAt < _clock.Now() && !cancellationToken.IsCancellationRequested)
        {
            foreach (var domainEventSnapshot in events)
            {
                var domainEvent = _domainEventFactory.Create(domainEventSnapshot);
                _events.Add(domainEvent, domainEventSnapshot);
                yield return domainEvent;
            }
            
            events = await GetBatchAsync(batchNumber++, cancellationToken);
        }
    }

    private Task<List<DomainEventSnapshot>> GetBatchAsync(int batchNumber, CancellationToken cancellationToken)
    {
        const int batchSize = 10;
        return _dbContext.Set<DomainEventSnapshot>()
            .Where(d => d.MetaData.Queue == Id)
            .OrderBy(d => d.MetaData.CreatedAt)
            .Skip(batchNumber * batchSize)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public Task MarkAsPublishedAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new())
    {
        return Task.CompletedTask;
    }

    public Task MarkAsFailedAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new()) => throw new NotImplementedException();
}