using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using SGP.Application.Interfaces;
using SGP.Application.Services;
using System.Reflection;

namespace SGP.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            Guard.Against.Null(services, nameof(services));

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICidadeService, CidadeService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            return services;
        }
    }
}