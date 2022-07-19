using System.IO.Compression;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SGP.Application;
using SGP.Infrastructure;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Middlewares;
using SGP.Shared;
using SGP.Shared.Extensions;
using StackExchange.Profiling;

namespace SGP.PublicApi;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddHttpClient()
            .AddHttpContextAccessor()
            .AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>())
            .AddMemoryCache()
            .AddApiVersioningAndApiExplorer()
            .AddOpenApi()
            .ConfigureAppSettings()
            .AddJwtBearer(_configuration, _environment)
            .AddServices()
            .AddInfrastructure()
            .AddRepositories()
            .AddSpgContext(services.AddHealthChecks().AddGCInfoCheck())
            .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
            .Configure<RouteOptions>(options => options.LowercaseUrls = true)
            .Configure<MvcNewtonsoftJsonOptions>(options => options.SerializerSettings.Configure())
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
                options.SuppressModelStateInvalidFilter = true;
            })
            .AddNewtonsoftJson();

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
    public static void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IMapper mapper,
        IApiVersionDescriptionProvider apiVersionProvider)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        ValidatorOptions.Global.Configure();
        mapper.ConfigurationProvider.AssertConfigurationIsValid();
        mapper.ConfigurationProvider.CompileMappings();

        app.UseMiddleware<ErrorHandlerMiddleware>()
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
            .UseEndpoints(endpoints => endpoints.MapControllers());
    }
}