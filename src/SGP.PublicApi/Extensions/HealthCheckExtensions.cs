using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    AllowCachingResponses = true,
                    ResponseWriter = (context, report) => context.Response.WriteAsync(report.ToJson())
                });
        }
    }
}