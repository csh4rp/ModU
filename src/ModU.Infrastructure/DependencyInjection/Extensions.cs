using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ModU.Infrastructure.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection Decorate<TService, TDecorator>(this IServiceCollection serviceCollection)
    {
        var serviceToWrap = serviceCollection.FirstOrDefault(s => s.ServiceType == typeof(TService));
        if (serviceToWrap is null)
        {
            throw new InvalidOperationException($"Service of type: '{typeof(TService)}' is not registered.");
        }

        var factory = ActivatorUtilities.CreateFactory(typeof(TDecorator), new[] {typeof(TService)});

        serviceCollection.Replace(ServiceDescriptor.Describe(typeof(TService),
            s => (TService) factory(s, new[] {s.CreateInstance(serviceToWrap)}), serviceToWrap.Lifetime));
        
        return serviceCollection;
    }

    private static object CreateInstance(this IServiceProvider serviceProvider, ServiceDescriptor serviceDescriptor)
    {
        if (serviceDescriptor.ImplementationInstance is not null)
        {
            return serviceDescriptor.ImplementationInstance;
        }

        return serviceDescriptor.ImplementationFactory is not null 
            ? serviceDescriptor.ImplementationFactory(serviceProvider)
            : ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, serviceDescriptor.ImplementationType);
    }
}