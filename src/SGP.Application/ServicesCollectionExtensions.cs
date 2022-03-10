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

            // Automatically register services ASP.NET Core DI container
            // REF: https://github.com/khellang/Scrutor
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.AssignableTo<IAppService>())
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}