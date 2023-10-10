using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.Abstractions;

namespace SGP.Application;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    private static readonly Assembly[] AssembliesToScan = { Assembly.GetExecutingAssembly() };

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // Add AutoMapper as a singleton instance
        services.AddSingleton<IMapper>(
            new AutoMapper.Mapper(new MapperConfiguration(cfg => cfg.AddMaps(AssembliesToScan))));

        // Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection
        // https://github.com/khellang/Scrutor
        services.Scan(scan => scan
            .FromAssemblies(AssembliesToScan)
            .AddClasses(impl => impl.AssignableTo<IAppService>())
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}