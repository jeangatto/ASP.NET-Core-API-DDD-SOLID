using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SGP.Application;
using SGP.Infrastructure;
using SGP.Infrastructure.Migrations;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;
using SGP.Shared.Extensions;
using System.Linq;
using System.Net.Mime;

namespace SGP.PublicApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddHttpContextAccessor();

            services.AddResponseCompression();

            services.AddOpenApi();

            services.AddApplication();

            services.AddInfrastructure();

            var healthChecksBuilder = services.AddHealthChecks();

            services.AddConfiguredDbContext(Configuration, healthChecksBuilder);

            services.ConfigureAppSettings(Configuration);

            services.Configure<RouteOptions>(routeOptions =>
            {
                routeOptions.LowercaseUrls = true;
                routeOptions.LowercaseQueryStrings = true;
            });

            services.Configure<KestrelServerOptions>(
                kestrelServerOptions => kestrelServerOptions.AddServerHeader = false);

            services.AddControllers()
                .ConfigureApiBehaviorOptions(apiBehaviorOptions =>
                {
                    apiBehaviorOptions.SuppressMapClientErrors = true;
                    apiBehaviorOptions.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson(jsonOptions =>
                {
                    var namingStrategy = new CamelCaseNamingStrategy();
                    jsonOptions.SerializerSettings.Formatting = Formatting.None;
                    jsonOptions.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    jsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver { NamingStrategy = namingStrategy };
                    jsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter(namingStrategy));
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
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

            app.UseHttpsRedirection();

            app.UseHsts();

            app.UseRouting();

            app.UseResponseCompression();

            app.UseAuthorization();

            app.UseCors(options =>
            {
                options.AllowAnyHeader();
                options.AllowAnyMethod();
                options.AllowAnyOrigin();
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
