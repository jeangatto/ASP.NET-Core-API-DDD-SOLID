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

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCors()
    .AddHttpContextAccessor()
    .AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>())
    .AddMemoryCache()
    .AddApiVersioningAndApiExplorer()
    .AddOpenApi()
    .ConfigureAppSettings()
    .AddJwtBearer(builder.Configuration)
    .AddServices()
    .AddInfrastructure()
    .AddRepositories()
    .AddDbContext(builder.Services.AddHealthChecks())
    .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal)
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

// MiniProfiler for .NET
// https://miniprofiler.com/dotnet/
builder.Services.AddMiniProfiler(options =>
{
    // Route: /profiler/results-index
    options.RouteBasePath = "/profiler";
    options.ColorScheme = ColorScheme.Dark;
}).AddEntityFramework();

builder.WebHost
    .UseKestrel(options => options.AddServerHeader = false)
    .UseDefaultServiceProvider((context, options) =>
    {
        options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
        options.ValidateOnBuild = true;
    });

var app = builder.Build();
app.Logger.LogInformation("PublicApi App created...");

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

ValidatorOptions.Global.Configure();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();
mapper.ConfigurationProvider.CompileMappings();

app.UseExceptionHandler(appBuilder =>
    {
        appBuilder.Run(async context =>
        {
            var handler = context.Features.Get<IExceptionHandlerFeature>();
            if (handler != null)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = MediaTypeNames.Application.Json;

                app.Logger.LogError(handler.Error, "Exceção não esperada: {Message}", handler.Error.Message);

                const string message = "Ocorreu um erro interno ao processar a sua solicitação.";
                var apiResponse = new ApiResponse(StatusCodes.Status500InternalServerError, message);
                await context.Response.WriteAsync(apiResponse.ToJson());
            }
        });
    })
    .UseOpenApi(app.Services.GetRequiredService<IApiVersionDescriptionProvider>())
    .UseHealthChecks()
    .UseForwardedHeaders()
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

app.Logger.LogInformation("Seeding Database...");
await app.MigrateDbContextAsync();

app.Logger.LogInformation("Launching PublicApi...");
await app.RunAsync();

#pragma warning disable CA1050
public partial class Program
{
}
#pragma warning restore CA1050