using System.Net.Mime;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using SGP.Application;
using SGP.Infrastructure;
using SGP.Infrastructure.Migrations;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;
using SGP.Shared;
using SGP.Shared.Extensions;

namespace SGP.PublicApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCors()
                .AddHttpContextAccessor()
                .AddResponseCompression()
                .AddMemoryCache()
                .AddApiVersioningAndApiExplorer()
                .AddOpenApi()
                .ConfigureAppSettings()
                .AddJwtBearer(_configuration)
                .AddServices()
                .AddInfrastructure()
                .AddDbContext(services.AddHealthChecks())
                .AddGraphQLWithSchemas()
                .Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                })
                .Configure<RouteOptions>(options =>
                {
                    options.LowercaseUrls = true;
                    options.LowercaseQueryStrings = true;
                })
                .AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson(options => options.SerializerSettings.Configure());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ILoggerFactory loggerFactory,
            IMapper mapper,
            IApiVersionDescriptionProvider apiVersionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ValidatorOptions.Global.Configure();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            mapper.ConfigurationProvider.CompileMappings();

            app.UseExceptionHandler(builder => ExceptionHandler(builder, loggerFactory))
                .UseOpenApi(apiVersionProvider)
                .UseHealthChecks()
                .UseForwardedHeaders()
                .UseGraphQL()
                .UseHttpsRedirection()
                .UseSerilogRequestLogging()
                .UseHsts()
                .UseRouting()
                .UseResponseCompression()
                .UseAuthentication()
                .UseAuthorization()
                .UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        #region ExceptionHandler

        /// <summary>
        /// Middleware nativo para tratamento de exceções.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="loggerFactory"></param>
        private static void ExceptionHandler(IApplicationBuilder builder, ILoggerFactory loggerFactory)
        {
            builder.Run(async context =>
            {
                var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
                if (exceptionHandler != null)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;

                    var logger = loggerFactory.CreateLogger<Startup>();
                    logger.LogError(exceptionHandler.Error, exceptionHandler.Error.Message);

                    var apiError = new ApiError("Ocorreu um erro interno ao processar a sua solicitação.");
                    var apiResponse = new ApiResponse(StatusCodes.Status500InternalServerError, apiError);
                    await context.Response.WriteAsync(apiResponse.ToJson());
                }
            });
        }

        #endregion
    }
}