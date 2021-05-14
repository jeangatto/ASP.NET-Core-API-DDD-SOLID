using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.Interfaces;
using System.Reflection;

namespace SGP.Application
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo<IService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}