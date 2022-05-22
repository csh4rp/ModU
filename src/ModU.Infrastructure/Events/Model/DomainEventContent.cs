using System.Text.Json;

namespace ModU.Infrastructure.Events.Model;

public sealed class DomainEventContent
{
    private DomainEventContent()
    {
    }
    
    public DomainEventContent(string name, string type, JsonDocument data)
    {
        Name = name;
        Type = type;
        Data = data;
    }

    public string Name { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public JsonDocument Data { get; private set; } = null!;
}