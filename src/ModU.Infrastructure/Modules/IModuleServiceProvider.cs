using ModU.Abstract.Modules;
using ModU.Infrastructure.Commands;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.Events.Stores;

namespace ModU.Infrastructure.Modules;

internal interface IModuleServiceProvider
{
    IUnitOfWork GetUnitOfWorkForModule(IModule module);

    BaseDbContext GetDbContextForModule(IModule module);

    IDomainQueueStore GetDomainQueueStoreForModule(IModule module);
}