using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Infrastructure.Services;
using SGP.Infrastructure.UoW;
using SGP.Shared.Interfaces;

namespace SGP.Infrastructure;

public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        => services
            .AddScoped<IDateTime, LocalDateTimeService>()
            .AddScoped<IHashService, BCryptHashService>()
            .AddScoped<ITokenClaimsService, IdentityTokenClaimService>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

    public static IServiceCollection AddRepositories(this IServiceCollection services)
        => services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(classes => classes.AssignableTo<IRepository>())
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());
}