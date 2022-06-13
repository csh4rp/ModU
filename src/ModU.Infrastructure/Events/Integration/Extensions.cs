using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Events.Integration;
using ModU.Abstract.Modules;

namespace ModU.Infrastructure.Events.Integration;

public static class Extensions
{
    public static IServiceCollection AddIntegrationEvents(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }

    public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection serviceCollection,
        IModule module)
    {
        var assembly = module.ApplicationAssembly();
        var integrationEventTypes = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IIntegrationEvent)));
        foreach (var eventType in integrationEventTypes)
        {
            var eventAttribute = eventType.GetCustomAttribute<IntegrationEventAttribute>();
            IntegrationEventTypeContainer.RegisterType(eventAttribute!.Name, eventType);
        }

        var handlerTypes = assembly.GetTypes().Where(t =>
            t.IsGenericTypeDefinition && t.GetGenericTypeDefinition().IsAssignableTo(typeof(IIntegrationEventHandler<>)));
        foreach (var handlerType in handlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()[0];
            serviceCollection.AddTransient(interfaceType, handlerType);
        }
        
        return serviceCollection;


        return serviceCollection;
    }
}