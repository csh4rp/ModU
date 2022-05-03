using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Commands;
using ModU.Infrastructure.Commands.Decorators;
using ModU.Infrastructure.DependencyInjection;

namespace ModU.Infrastructure.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection serviceCollection, Assembly[] assemblies)
    {
        serviceCollection.AddSingleton<ICommandProcessor, CommandProcessor>();
        
        foreach (var assembly in assemblies)
        {
            var handlerTypesWithoutResult = assembly.GetTypes()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableTo(typeof(ICommandHandler<>)));
            
            var handlerTypesWithResult = assembly.GetTypes()
                .Where(t => t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableTo(typeof(ICommandHandler<,>)));

            foreach (var type in handlerTypesWithoutResult)
            {
                var @interface = type.GetInterfaces().Single();
                var genericTypeArgument = type.GetGenericArguments().Single();
                serviceCollection.AddTransient(@interface, type);
                serviceCollection.Decorate(@interface, typeof(TransactionalDecorator<>).MakeGenericType(genericTypeArgument));
            }
            
            foreach (var type in handlerTypesWithResult)
            {
                var @interface = type.GetInterfaces().Single();
                var genericTypeArguments = type.GetGenericArguments();
                serviceCollection.AddTransient(@interface, type);
                serviceCollection.Decorate(@interface, typeof(TransactionalDecorator<>).MakeGenericType(genericTypeArguments));
            }
        }

        return serviceCollection;
    }
}