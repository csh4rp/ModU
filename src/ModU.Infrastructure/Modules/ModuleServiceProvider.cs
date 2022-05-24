using Microsoft.Extensions.DependencyInjection;
using ModU.Infrastructure.Commands;
using ModU.Infrastructure.Database;

namespace ModU.Infrastructure.Modules;

internal sealed class ModuleServiceProvider : IModuleServiceProvider
{
    private readonly ModuleNameResolver _moduleNameResolver = new();
    private readonly IServiceProvider _serviceProvider;

    public ModuleServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public IUnitOffWork GetUnitOfWorkForType(Type typeFromModule)
    {
        var moduleName = _moduleNameResolver.Resolve(typeFromModule.FullName!);
        var type = ModuleTypeRegistry.Instance.GetUnitOfWorkType(moduleName);
        return (IUnitOffWork) _serviceProvider.GetRequiredService(type);
    }

    public BaseDbContext GetDbContextForType(Type typeFromModule)
    {
        var moduleName = _moduleNameResolver.Resolve(typeFromModule.FullName!);
        var type = ModuleTypeRegistry.Instance.GetContextType(moduleName);
        return (BaseDbContext) _serviceProvider.GetRequiredService(type);
    }
}