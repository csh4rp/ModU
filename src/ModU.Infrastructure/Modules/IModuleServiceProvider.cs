using ModU.Infrastructure.Commands;
using ModU.Infrastructure.Database;

namespace ModU.Infrastructure.Modules;

internal interface IModuleServiceProvider
{
    IUnitOffWork GetUnitOfWorkForType(Type typeFromModule);

    BaseDbContext GetDbContextForType(Type typeFromModule);
}