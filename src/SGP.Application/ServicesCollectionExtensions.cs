using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using SGP.Shared.Interfaces;

namespace SGP.Application
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(@class => @class.AssignableTo<IAppService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}