using ModU.Infrastructure.Messaging.Model;
using ModU.Infrastructure.Modules;

namespace ModU.Infrastructure.Messaging;

internal sealed class Outbox : IOutbox
{
    private readonly ModuleServiceProvider _moduleServiceProvider;

    public Outbox(ModuleServiceProvider moduleServiceProvider) => _moduleServiceProvider = moduleServiceProvider;


    public Task SaveAsync(object message)
    {
        var context = _moduleServiceProvider.GetDbContextForType(message.GetType());
        return Task.CompletedTask;
    }
}