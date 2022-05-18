using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.Interfaces;

namespace SGP.Application;

public static class ServicesCollectionExtensions
{
    private static readonly Assembly[] Assemblies = new[] { Assembly.GetExecutingAssembly() };

    public static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddAutoMapper(cfg => cfg.DisableConstructorMapping(), Assemblies, ServiceLifetime.Scoped)
            .Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo<IAppService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
}