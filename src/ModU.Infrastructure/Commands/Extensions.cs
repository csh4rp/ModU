using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Commands;
using ModU.Infrastructure.Database;
using ModU.Infrastructure.DependencyInjection;

namespace ModU.Infrastructure.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection, Assembly assembly, Type unitOfWorkType)
    {
        if (!unitOfWorkType.IsAssignableTo(typeof(BaseDbContext)))
        {
            throw new InvalidOperationException($"Cannot register type: '{unitOfWorkType}' as context.");
        }

        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableTo(typeof(ICommandHandler<>)));

        foreach (var type in handlerTypes)
        {
            var @interface = type.GetInterfaces().Single();
            var genericType = type.GetGenericArguments()[0];
            serviceCollection.AddTransient(@interface, type);
            var decoratorType = typeof(ICommandHandler<>).MakeGenericType(genericType);
            serviceCollection.Decorate(type, decoratorType);
        }

        return serviceCollection;
    }
}