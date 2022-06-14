namespace ModU.Abstract.Events.Integration;

public interface IEventMapping<in TDomainEvent, out TIntegrationEvent>
{
    TIntegrationEvent Map(TDomainEvent domainEvent);
}