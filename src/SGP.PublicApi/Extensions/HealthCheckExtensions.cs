using System.Diagnostics.CodeAnalysis;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace SGP.PublicApi.Extensions;

[ExcludeFromCodeCoverage]
internal static class HealthCheckExtensions
{
    internal static void UseHealthChecks(this IApplicationBuilder app) =>
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
}