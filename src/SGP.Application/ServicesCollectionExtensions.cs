using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.Interfaces;

namespace SGP.Application;

public static class ServicesCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
        => services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo<IAppService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
}