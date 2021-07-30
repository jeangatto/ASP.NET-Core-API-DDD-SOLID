using System.Reflection;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.Interfaces;

namespace SGP.Application
{
    public static class ServicesCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(implementations => implementations.AssignableTo<IAppService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }
    }
}