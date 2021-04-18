using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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

            services.AddApiVersioningAndApiExplorer();

            services.AddOpenApi();

            services.AddApplication();

            services.AddInfrastructure();

            var healthChecksBuilder = services.AddHealthChecks();

            services.AddDbContext(Configuration, healthChecksBuilder);

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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi(provider);

            app.UseHealthChecks();

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
