using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ModU.Infrastructure.Database;

public static class Extensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t.Name.EndsWith("Repository"))
            .ToList();

        foreach (var repositoryType in types)
        {
            var interfaceType = repositoryType.GetInterfaces().Single();
            serviceCollection.AddScoped(interfaceType, repositoryType);
        }
        
        return serviceCollection;
    }
}