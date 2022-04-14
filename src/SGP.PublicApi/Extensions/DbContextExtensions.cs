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

    internal static IServiceCollection AddDbContext(this IServiceCollection services,
        IHealthChecksBuilder healthChecksBuilder)
    {
        services.AddDbContext<SgpContext>((provider, builder) =>
        {
            builder.UseSqlServer(provider.GetConnectionString(),
                options => options.MigrationsAssembly(AssemblyName).EnableRetryOnFailure());

            // NOTE: Quando for ambiente de desenvolvimento será logado informações detalhadas.
            var environment = provider.GetRequiredService<IHostEnvironment>();
            if (environment.IsDevelopment())
                builder.EnableDetailedErrors().EnableSensitiveDataLogging();
        });

        // Verificador de saúde da base de dados.
        healthChecksBuilder.AddDbContextCheck<SgpContext>(
            tags: new[] {"database"},
            customTestQuery: (context, token) => context.Cidades.AsNoTracking().AnyAsync(token));

        return services;
    }

    private static string GetConnectionString(this IServiceProvider provider)
        => provider.GetRequiredService<IOptions<ConnectionStrings>>().Value.DefaultConnection;
}