using Microsoft.Extensions.DependencyInjection;
using SGP.Application.Interfaces;
using SGP.Application.Services;
using System.Reflection;

namespace SGP.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IAuthAppService, AuthAppService>();
            services.AddScoped<ICityAppService, CityAppService>();
            services.AddScoped<IUserAppService, UserAppService>();
            return services;
        }
    }
}