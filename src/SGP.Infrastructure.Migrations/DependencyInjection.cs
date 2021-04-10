using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;
using System.Reflection;

namespace SGP.Infrastructure.Migrations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContextWithMigrations(this IServiceCollection services, IConfiguration configuration)
        {
            const string connectionName = nameof(ConnectionStrings.DefaultConnection);
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            services.AddDbContext<SgpContext>(options
                => options.UseSqlServer(configuration.GetConnectionString(connectionName),
                       sqlServer => sqlServer.MigrationsAssembly(assemblyName)));

            return services;
        }
    }
}