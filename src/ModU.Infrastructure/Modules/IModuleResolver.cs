using ModU.Abstract.Modules;

namespace ModU.Infrastructure.Modules;

public interface IModuleResolver
{
    IModule ResolveForType(Type type);
}