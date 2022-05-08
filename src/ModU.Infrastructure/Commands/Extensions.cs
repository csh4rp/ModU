﻿using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Commands;
using ModU.Abstract.Commands.Attributes;
using ModU.Infrastructure.Commands.Decorators;
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

        var registry = new UnitOfWorkTypeRegistry();
        
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.IsGenericType && t.GetGenericTypeDefinition().IsAssignableTo(typeof(ICommandHandler<>)));

        foreach (var type in handlerTypes)
        {
            var @interface = type.GetInterfaces().Single();
            var genericTypeArgument = type.GetGenericArguments().Single();
            var transactionalAttribute = type.GetCustomAttribute<TransactionalAttribute>();
            serviceCollection.AddTransient(@interface, type);
            registry.Add(@interface, unitOfWorkType);

            if (transactionalAttribute is not null)
            {
                serviceCollection.Decorate(@interface,
                    typeof(TransactionalDecorator<>).MakeGenericType(genericTypeArgument));
            }
        }

        serviceCollection.AddSingleton(registry);

        return serviceCollection;
    }
}