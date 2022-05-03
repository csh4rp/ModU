using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModU.Abstract.Modules;

public interface IModule
{
    string Name { get; }

    void Use(IApplicationBuilder app);

    void Register(IServiceCollection serviceCollection, IConfiguration configuration);
}