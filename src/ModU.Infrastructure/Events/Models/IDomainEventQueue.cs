using ModU.Infrastructure.Events.Entities;

namespace ModU.Infrastructure.Events.Models;

public interface IDomainEventQueue
{
    string Id { get; }

    bool TryDequeue(out DomainEventSnapshot? domainEventSnapshot);

}