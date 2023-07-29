using System;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using AutoMapper;
using FluentValidation;
using FluentValidation.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SGP.Application;
using SGP.Infrastructure;
using SGP.Infrastructure.Data.Context;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.Middlewares;
using SGP.Shared;
using SGP.Shared.AppSettings;
using SGP.Shared.Extensions;
using StackExchange.Profiling;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .Configure<GzipCompressionProviderOptions>(compressionOptions => compressionOptions.Level = CompressionLevel.Optimal)
    .Configure<KestrelServerOptions>(kestrelOptions => kestrelOptions.AddServerHeader = false)
    .Configure<MvcNewtonsoftJsonOptions>(jsonOptions => jsonOptions.SerializerSettings.Configure())
    .Configure<RouteOptions>(routeOptions => routeOptions.LowercaseUrls = true)
    .AddHttpClient()
    .AddHttpContextAccessor()
    .AddResponseCompression(compressionOptions =>
    {
        compressionOptions.EnableForHttps = true;
        compressionOptions.Providers.Add<GzipCompressionProvider>();
    })
    .AddCache(builder.Configuration)
    .AddApiVersioning(versioningOptions =>
    {
        // Specify the default API Version as 1.0
        versioningOptions.DefaultApiVersion = ApiVersion.Default;
        // Reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
        versioningOptions.ReportApiVersions = true;
        // If the client hasn't specified the API version in the request, use the default API version number
        versioningOptions.AssumeDefaultVersionWhenUnspecified = true;
    })
    .AddVersionedApiExplorer(explorerOptions =>
    {
        // Add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // NOTE: the specified format code will format the version as "'v'major[.minor][-status]"
        explorerOptions.GroupNameFormat = "'v'VVV";
        // NOTE: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        explorerOptions.SubstituteApiVersionInUrl = true;
    })
    .AddOpenApi()
    .ConfigureAppSettings()
    .AddJwtBearer(builder.Configuration, builder.Environment.IsProduction())
    .AddServices()
    .AddInfrastructure()
    .AddRepositories()
    .AddSpgContext(builder.Services.AddHealthChecks().AddGCInfoCheck());

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
        options.SuppressModelStateInvalidFilter = true;
    }).AddNewtonsoftJson();

// MiniProfiler for .NET
// https://miniprofiler.com/dotnet/
builder.Services.AddMiniProfiler(options =>
{
    // Route: /profiler/results-index
    options.RouteBasePath = "/profiler";
    options.ColorScheme = ColorScheme.Dark;
    options.EnableServerTimingHeader = true;
    options.EnableDebugMode = builder.Environment.IsDevelopment();
    options.TrackConnectionOpenClose = true;
}).AddEntityFramework();

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    options.ValidateOnBuild = true;
});

builder.WebHost.UseKestrel(kestrelOptions => kestrelOptions.AddServerHeader = false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// Configuração global do FluentValidation.
ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => member?.Name;
ValidatorOptions.Global.LanguageManager = new LanguageManager { Culture = new CultureInfo("pt-Br") };

app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseSwaggerAndUI(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
app.UseHealthChecks();
app.UseHttpsRedirection();
app.UseHsts();
app.UseRouting();
app.UseHttpLogging();
app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiniProfiler();
app.MapControllers();

await using var serviceScope = app.Services.CreateAsyncScope();
await using var context = serviceScope.ServiceProvider.GetRequiredService<SgpContext>();
var mapper = serviceScope.ServiceProvider.GetRequiredService<IMapper>();
var inMemoryOptions = serviceScope.ServiceProvider.GetOptions<InMemoryOptions>();

try
{
    app.Logger.LogInformation("----- AutoMapper: Validando os mapeamentos...");

    mapper.ConfigurationProvider.AssertConfigurationIsValid();
    mapper.ConfigurationProvider.CompileMappings();

    app.Logger.LogInformation("----- AutoMapper: Mapeamentos são válidos!");

    if (inMemoryOptions.Database)
    {
        app.Logger.LogInformation("----- Database InMemory: Criando e migrando a base de dados...");
        await context.Database.EnsureCreatedAsync();
    }
    else
    {
        var connectionString = context.Database.GetConnectionString();
        app.Logger.LogInformation("----- SQL Server: {Connection}", connectionString);
        app.Logger.LogInformation("----- SQL Server: Verificando se existem migrações pendentes...");

        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            app.Logger.LogInformation("----- SQL Server: Criando e migrando a base de dados...");

            await context.Database.MigrateAsync();

            app.Logger.LogInformation("----- SQL Server: Base de dados criada e migrada com sucesso!");
        }
        else
        {
            app.Logger.LogInformation("----- SQL Server: Migrações estão em dia");
        }
    }

    app.Logger.LogInformation("----- Populando a base de dados...");

    await context.EnsureSeedDataAsync();

    app.Logger.LogInformation("----- Base de dados populada com sucesso!");
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "Ocorreu uma exceção ao iniciar a aplicação: {Message}", ex.Message);
    throw;
}

app.Logger.LogInformation("----- Iniciado a aplicação...");
app.Run();

#pragma warning disable CA1050
namespace SGP.PublicApi
{
#pragma warning disable S2094
    public class Program
    {
    }
#pragma warning restore S2094
}
#pragma warning restore CA1050