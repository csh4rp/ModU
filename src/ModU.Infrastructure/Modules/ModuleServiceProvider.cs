using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Modules;
using ModU.Abstract.Time;
using ModU.Infrastructure.Commands;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.Events.Stores;

namespace ModU.Infrastructure.Modules;

internal sealed class ModuleServiceProvider : IModuleServiceProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IClock _clock;

    public ModuleServiceProvider(IServiceProvider serviceProvider, IClock clock)
    {
        _serviceProvider = serviceProvider;
        _clock = clock;
    }

    public IUnitOfWork GetUnitOfWorkForModule(IModule module)
    {
        var type = ModuleTypeRegistry.Instance.GetUnitOfWorkType(module.Name);
        return (IUnitOfWork) _serviceProvider.GetRequiredService(type);
    }

    public BaseDbContext GetDbContextForModule(IModule module)
    {
        var type = ModuleTypeRegistry.Instance.GetContextType(module.Name);
        return (BaseDbContext) _serviceProvider.GetRequiredService(type);
    }

    public IDomainQueueStore GetDomainQueueStoreForModule(IModule module)
    {
        var dbContext = GetDbContextForModule(module);
        var snapshotStore = new DomainEventSnapshotStore(dbContext, _clock);
        var lockStore = new DomainEventQueueLockStore(dbContext);
        return new DomainQueueStore(lockStore, snapshotStore, _clock);
    }
}