using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using SGP.Shared.Extensions;

namespace SGP.PublicApi.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health",
                new HealthCheckOptions
                {
                    AllowCachingResponses = true,
                    ResponseWriter = (context, healthReport) => context.Response.WriteAsync(healthReport.ToJson())
                });

            return app;
        }
    }
}