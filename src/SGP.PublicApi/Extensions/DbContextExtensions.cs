using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SGP.Infrastructure.Context;
using SGP.Shared.AppSettings;

namespace SGP.PublicApi.Extensions;

internal static class DbContextExtensions
{
    private static readonly string AssemblyName = typeof(Program).Assembly.GetName().Name;

    internal static IServiceCollection AddSpgContext(this IServiceCollection services,
        IHealthChecksBuilder healthChecksBuilder)
    {
        services.AddDbContext<SgpContext>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(serviceProvider.GetConnectionString(), sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(AssemblyName);

                // Configurando a resiliência da conexão: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 3);
            });

            // NOTE: Quando for ambiente de desenvolvimento será logado informações detalhadas.
            var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
                optionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();
        });

        // Verificador de saúde da base de dados.
        healthChecksBuilder.AddDbContextCheck<SgpContext>(
            tags: new[] { "database" },
            customTestQuery: (context, token) => context.Cidades.AsNoTracking().AnyAsync(token));

        return services;
    }

    private static string GetConnectionString(this IServiceProvider provider)
        => provider.GetRequiredService<IOptions<ConnectionStrings>>().Value.DefaultConnection;
}