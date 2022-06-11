using System.Text.Json;
using ModU.Abstract.Events.Domain;
using ModU.Infrastructure.Events.Domain.Entities;

namespace ModU.Infrastructure.Events.Domain.Factories;

internal sealed class DomainEventFactory : IDomainEventFactory
{
    public IDomainEvent Create(DomainEventSnapshot snapshot)
    {
        var type = Type.GetType(snapshot.Type);
        return (IDomainEvent)snapshot.Data.Deserialize(type!)!;
    }
}