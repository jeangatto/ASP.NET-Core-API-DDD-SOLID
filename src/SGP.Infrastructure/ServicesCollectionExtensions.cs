using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Infrastructure.Services;
using SGP.Infrastructure.UoW;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure;

public static class ServicesCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDateTime, LocalDateTimeService>();
        services.AddScoped<IHashService, BCryptHashService>();
        services.AddScoped<ITokenClaimsService, IdentityTokenClaimService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.AssignableTo<IRepository>())
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }
}