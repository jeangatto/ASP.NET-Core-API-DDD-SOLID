using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Extensions;

internal static class HealthCheckExtensions
{
    internal static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        => app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = (_) => true,
            ResponseWriter = (context, healthReport) => context.Response.WriteAsync(healthReport.ToJson())
        });
}