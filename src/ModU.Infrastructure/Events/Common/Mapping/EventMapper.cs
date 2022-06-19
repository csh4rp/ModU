using ModU.Abstract.Events.Domain;
using ModU.Abstract.Events.Integration;

namespace ModU.Infrastructure.Events.Common.Mapping;

internal sealed class EventMapper : IEventMapper
{
    private static readonly Dictionary<Type, object> Mappers = new();
    
    public IEnumerable<IIntegrationEvent> Map(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var type = domainEvent.GetType();
            if (!Mappers.TryGetValue(type, out var mapper))
            {
                continue;
            }

            yield return ((dynamic) mapper).Map(domainEvent);
        }
    }
}