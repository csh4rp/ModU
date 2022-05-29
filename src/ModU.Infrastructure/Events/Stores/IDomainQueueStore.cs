namespace ModU.Infrastructure.Events.Stores;

public interface IDomainQueueStore
{
    Task<IReadOnlyCollection<IDomainQueueStore>> GetQueuesToProcessAsync(CancellationToken cancellationToken = new());
}