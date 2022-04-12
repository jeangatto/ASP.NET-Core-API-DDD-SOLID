using System.IO.Compression;
using System.Net.Mime;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Application;
using SGP.Infrastructure;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Models;
using SGP.Shared;
using SGP.Shared.Extensions;
using StackExchange.Profiling;

namespace SGP.PublicApi;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCors()
            .AddHttpClient()
            .AddHttpContextAccessor()
            .AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>())
            .AddMemoryCache()
            .AddApiVersioningAndApiExplorer()
            .AddOpenApi()
            .ConfigureAppSettings()
            .AddJwtBearer(_configuration)
            .AddServices()
            .AddInfrastructure()
            .AddRepositories()
            .AddDbContext(services.AddHealthChecks())
            .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
            .Configure<RouteOptions>(options => options.LowercaseUrls = true)
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddNewtonsoftJson(options => options.SerializerSettings.Configure());

        // MiniProfiler for .NET
        // https://miniprofiler.com/dotnet/
        services.AddMiniProfiler(options =>
        {
            // Route: /profiler/results-index
            options.RouteBasePath = "/profiler";
            options.ColorScheme = ColorScheme.Dark;
        }).AddEntityFramework();
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
            app.UseDeveloperExceptionPage();

        ValidatorOptions.Global.Configure();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        mapper.ConfigurationProvider.CompileMappings();

        app.UseExceptionHandler(builder => ExceptionHandler(builder, loggerFactory))
            .UseOpenApi(apiVersionProvider)
            .UseHealthChecks()
            .UseHttpsRedirection()
            .UseHsts()
            .UseRouting()
            .UseHttpLogging()
            .UseResponseCompression()
            .UseAuthentication()
            .UseAuthorization()
            .UseMiniProfiler()
            .UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }

    #region ExceptionHandler

    private static readonly ApiError ApiDefaultError =
        new("Ocorreu um erro interno ao processar a sua solicitação.");

    /// <summary>
    /// Middleware nativo para tratamento de exceções.
    /// </summary>
    /// <param name="applicationBuilder"></param>
    /// <param name="loggerFactory"></param>
    private static void ExceptionHandler(IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory)
        => applicationBuilder.Run(async httpContext =>
        {
            var handler = httpContext.Features.Get<IExceptionHandlerFeature>();
            if (handler != null)
            {
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;

                var logger = loggerFactory.CreateLogger<Startup>();
                logger.LogError(handler.Error, "Exceção não esperada: {Message}", handler.Error.Message);

                var apiResponse = new ApiResponse(StatusCodes.Status500InternalServerError, ApiDefaultError);
                await httpContext.Response.WriteAsync(apiResponse.ToJson());
            }
        });

    #endregion
}