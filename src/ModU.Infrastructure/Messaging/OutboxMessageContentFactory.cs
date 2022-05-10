using System.Reflection;
using System.Text.Json;
using ModU.Abstract.Messaging;
using ModU.Infrastructure.Messaging.Model;

namespace ModU.Infrastructure.Messaging;

internal abstract class OutboxMessageContentFactory
{
    public static OutboxMessageContent Create(object obj)
    {
        var objType = obj.GetType();
        var messageName = GetMessageName(objType);
        var data = GetData(obj);
        return new OutboxMessageContent(messageName, objType.FullName!, data);
    }
    
    private static string GetMessageName(Type objType)
    {
        var attribute = objType.GetCustomAttribute<MessageAttribute>();
        return attribute is null
            ? objType.FullName!
            : attribute.Name;
    }

    private static JsonDocument GetData(object obj) => JsonSerializer.SerializeToDocument(obj);
}