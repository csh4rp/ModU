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

public sealed class DomainEventQueue : IDomainEventQueue
{
    private readonly Dictionary<IDomainEvent, DomainEventSnapshot> _events = new();
    private readonly List<DomainEventSnapshot> _snapshots = new();
    private readonly BaseDbContext _dbContext;
    private readonly IClock _clock;
    private readonly IDomainEventFactory _domainEventFactory;
    private readonly IOptions<DomainEventOptions> _options;
    private readonly DomainEventQueueLock _domainEventQueueLock;

    public DomainEventQueue(BaseDbContext dbContext, IClock clock, IDomainEventFactory domainEventFactory,
        IOptions<DomainEventOptions> options, string id)
    {
        _dbContext = dbContext;
        _clock = clock;
        _domainEventFactory = domainEventFactory;
        _options = options;
        Id = id;
    }

    public string Id { get; }

    public async ValueTask DisposeAsync()
    {

    }
    
    public async IAsyncEnumerable<IDomainEvent> GetEventsAsync([EnumeratorCancellation] CancellationToken cancellationToken = new())
    {
        if (_domainEventQueueLock is null)
        {
            throw new InvalidOperationException("A lock has to be acquired before events can be fetched.");
        }

        var batchIndex = 0;
        var events = await GetBatchAsync(batchIndex, cancellationToken);
        while (events.Any())
        {
            foreach (var domainEventSnapshot in events)
            {
                if (ShouldOperationBeCancelled(cancellationToken))
                {
                    yield break;
                }
                
                var domainEvent = _domainEventFactory.Create(domainEventSnapshot);
                _events.Add(domainEvent, domainEventSnapshot);
                yield return domainEvent;
            }
            
            events = await GetBatchAsync(batchIndex++, cancellationToken);
        }
    }

    private bool ShouldOperationBeCancelled(CancellationToken cancellationToken)
        => cancellationToken.IsCancellationRequested || _domainEventQueueLock.ExpiresAt < _clock.Now();
    
    private Task<List<DomainEventSnapshot>> GetBatchAsync(int batchIndex, CancellationToken cancellationToken)
    {
        const int batchSize = 50;
        return _dbContext.Set<DomainEventSnapshot>()
            .Where(d => d.Queue == Id && !d.FailedAt.HasValue)
            .OrderBy(d => d.CreatedAt)
            .Skip(batchIndex * batchSize)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async ValueTask MarkAsDeliveredAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new())
    {
        var snapshot = _events[domainEvent];
        snapshot.MarkAsDelivered(_clock.Now());
        if (_options.Value.UseBatching)
        {
            _snapshots.Add(snapshot);
        }
        else
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async ValueTask MarkAsFailedAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = new())
    {
        var snapshot = _events[domainEvent];
        snapshot.AttemptFailed(_clock.Now().AddSeconds(snapshot.FailedAttempts * 5));
        if (_options.Value.UseBatching)
        {
            _snapshots.Add(snapshot);
        }
        else
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}