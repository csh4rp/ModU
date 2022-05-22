﻿using System.Text.Json;

namespace ModU.Infrastructure.Messaging.Model;

public class OutboxMessageContent
{
    private OutboxMessageContent()
    {
    }

    public OutboxMessageContent(string name, string type, JsonDocument data)
    {
        Name = name;
        Type = type;
        Data = data;
    }

    public string Name { get; private set; } = null!;
    public string Type { get; private set; } = null!;
    public JsonDocument Data { get; private set; } = null!;
}