namespace ModU.Infrastructure.Events.Domain;

internal sealed class DomainEventTypeContainer
{
    private static readonly Dictionary<string, Type> _types = new();

    public static void  RegisterType(string eventName, Type type)
    {
        _types.Add(eventName, type);
    }

    public static Type GetType(string eventName) => _types[eventName];
}