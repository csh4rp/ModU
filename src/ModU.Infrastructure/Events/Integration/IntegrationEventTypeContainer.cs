namespace ModU.Infrastructure.Events.Integration;

internal sealed class IntegrationEventTypeContainer
{
    private static readonly Dictionary<string, List<Type>> Types = new();  

    public static void RegisterType(string eventType, Type type)
    {
        if (Types.TryGetValue(eventType, out var types))
        {
            types.Add(type);
            return;
        }
        
        Types.Add(eventType, new List<Type>{type});
    }

    public static IReadOnlyCollection<Type> GetTypes(string eventName)
        => Types[eventName];

}