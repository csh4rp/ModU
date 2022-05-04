using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Commands;
using ModU.Abstract.Commands.Attributes;
using ModU.Infrastructure.Commands.Decorators;
using ModU.Infrastructure.DependencyInjection;

namespace ModU.Infrastructure.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection, Assembly assembly)
    {
        serviceCollection.AddSingleton<ICommandProcessor, CommandProcessor>();

        var handlerTypesWithoutResult = assembly.GetTypes()
            .Where(t => t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableTo(typeof(ICommandHandler<>)));

        var handlerTypesWithResult = assembly.GetTypes()
            .Where(t => t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableTo(typeof(ICommandHandler<,>)));

        foreach (var type in handlerTypesWithoutResult)
        {
            var @interface = type.GetInterfaces().Single();
            var genericTypeArgument = type.GetGenericArguments().Single();
            var transactionalAttribute = type.GetCustomAttribute<TransactionalAttribute>();
            serviceCollection.AddTransient(@interface, type);

            if (transactionalAttribute is not null)
            {
                serviceCollection.Decorate(@interface,
                    typeof(TransactionalDecorator<>).MakeGenericType(genericTypeArgument));
            }
        }

        foreach (var type in handlerTypesWithResult)
        {
            var @interface = type.GetInterfaces().Single();
            var genericTypeArguments = type.GetGenericArguments();
            serviceCollection.AddTransient(@interface, type);
            var transactionalAttribute = type.GetCustomAttribute<TransactionalAttribute>();

            if (transactionalAttribute is not null)
            {
                serviceCollection.Decorate(@interface,
                    typeof(TransactionalDecorator<>).MakeGenericType(genericTypeArguments));
            }
        }

        return serviceCollection;
    }
}