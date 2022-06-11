using ModU.Abstract.Events.Common;

namespace ModU.Abstract.Events.Contracts;

public interface IEventContract<TEvent> where TEvent : IEvent
{
    EventContractValidationResult Validate(Type type);
}