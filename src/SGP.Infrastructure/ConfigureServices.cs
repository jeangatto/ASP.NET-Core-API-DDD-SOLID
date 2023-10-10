using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Domain.Repositories;
using SGP.Infrastructure.Data;
using SGP.Infrastructure.Data.Repositories.Cached;
using SGP.Infrastructure.Services;
using SGP.Shared.Abstractions;

namespace SGP.Infrastructure;

[ExcludeFromCodeCoverage]
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddScoped<IDateTimeService, DateTimeService>()
            .AddScoped<IHashService, BCryptHashService>()
            .AddScoped<ITokenClaimsService, JwtClaimService>()
            .AddScoped<IUnitOfWork, UnitOfWork>();

    public static IServiceCollection AddMemoryCacheService(this IServiceCollection services) =>
        services.AddScoped<ICacheService, MemoryCacheService>();

    public static IServiceCollection AddDistributedCacheService(this IServiceCollection services) =>
        services.AddScoped<ICacheService, DistributedCacheService>();

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Assembly scanning and decoration extensions for Microsoft.Extensions.DependencyInjection
        // https://github.com/khellang/Scrutor
        services.Scan(scan => scan
            .FromCallingAssembly()
            .AddClasses(impl => impl.AssignableTo<IRepository>())
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // The decorator pattern
        // REF: https://andrewlock.net/adding-decorated-classes-to-the-asp.net-core-di-container-using-scrutor/
        services
            .Decorate<ICidadeRepository, CidadeCachedRepository>()
            .Decorate<IEstadoRepository, EstadoCachedRepository>()
            .Decorate<IRegiaoRepository, RegiaoCachedRepository>();

        return services;
    }
}