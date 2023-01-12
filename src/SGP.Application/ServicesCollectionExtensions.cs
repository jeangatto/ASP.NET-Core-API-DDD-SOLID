using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.Interfaces;

namespace SGP.Application;

[ExcludeFromCodeCoverage]
public static class ServicesCollectionExtensions
{
    private static readonly Assembly[] AssembliesToScan = { Assembly.GetExecutingAssembly() };

    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddAutoMapper(cfg => cfg.DisableConstructorMapping(), AssembliesToScan, ServiceLifetime.Scoped)
            .Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(impl => impl.AssignableTo<IAppService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
}