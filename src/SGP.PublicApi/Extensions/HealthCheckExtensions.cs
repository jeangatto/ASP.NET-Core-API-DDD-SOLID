using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using SGP.PublicApi.Models;
using SGP.Shared.Extensions;
using System.Linq;
using System.Net.Mime;

namespace SGP.PublicApi.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            Guard.Against.Null(app, nameof(app));

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = true,
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var healthCheckReponse = new HealthCheckReponse
                    {
                        Status = report.Status.ToString(),
                        HealthCheckDuration = report.TotalDuration,
                        HealthChecks = report.Entries.Select(entry => new IndividualHealthCheckResponse
                        {
                            Components = entry.Key,
                            Status = entry.Value.Status.ToString(),
                            Description = entry.Value.Description
                        })
                    };

                    await context.Response.WriteAsync(healthCheckReponse.ToJson());
                }
            });

            return app;
        }
    }
}