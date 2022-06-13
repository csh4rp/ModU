using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ModU.Abstract.Events.Domain;
using ModU.Abstract.Modules;

namespace ModU.Infrastructure.Events.Domain;

public static class Extensions
{
    public static IServiceCollection AddDomainEvents(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }

    public static IServiceCollection AddDomainEventHandlers(this IServiceCollection serviceCollection, IModule module)
    {
        var assembly = module.ApplicationAssembly();
        var domainEventTypes = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IDomainEvent)));
        foreach (var domainEventType in domainEventTypes)
        {
            var eventAttribute = domainEventType.GetCustomAttribute<DomainEventAttribute>();
            DomainEventTypeContainer.RegisterType(eventAttribute!.Name, domainEventType);
        }

        var handlerTypes = assembly.GetTypes().Where(t =>
            t.IsGenericTypeDefinition && t.GetGenericTypeDefinition().IsAssignableTo(typeof(IDomainEventHandler<>)));
        foreach (var handlerType in handlerTypes)
        {
            var interfaceType = handlerType.GetInterfaces()[0];
            serviceCollection.AddTransient(interfaceType, handlerType);
        }
        
        return serviceCollection;
    }

    public static IApplicationBuilder UseDomainEvents(this IApplicationBuilder applicationBuilder)
    {
        
        
        return applicationBuilder;
    }
}