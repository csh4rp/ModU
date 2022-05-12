using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Database;
using ModU.Infrastructure.Commands;

namespace ModU.Infrastructure.Modules;

internal sealed class ModuleServiceProvider
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

    public IDbContext GetDbContextForType(Type typeFromModule)
    {
        var moduleName = _moduleNameResolver.Resolve(typeFromModule.FullName!);
        var type = ModuleTypeRegistry.Instance.GetContextType(moduleName);
        return (IDbContext) _serviceProvider.GetService(type);
    }
}