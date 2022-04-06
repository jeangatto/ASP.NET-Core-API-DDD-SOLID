using System.IO.Compression;
using System.Net.Mime;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());
builder.Services.AddMemoryCache();
builder.Services.AddOpenApi();
builder.Services.ConfigureAppSettings();
builder.Services.AddJwtBearer(builder.Configuration);
builder.Services.AddServices();
builder.Services.AddInfrastructure();
builder.Services.AddRepositories();
builder.Services.AddDbContext(builder.Services.AddHealthChecks());

builder.Services.AddApiVersioning(options =>
{
    // Specify the default API Version as 1.0
    options.DefaultApiVersion = ApiVersion.Default;
    // Reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
    options.ReportApiVersions = true;
    // If the client hasn't specified the API version in the request, use the default API version number
    options.AssumeDefaultVersionWhenUnspecified = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    // Add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // NOTE: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";
    // NOTE: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});

// MiniProfiler for .NET
// https://miniprofiler.com/dotnet/
builder.Services.AddMiniProfiler(options =>
{
    // Route: /profiler/results-index
    options.RouteBasePath = "/profiler";
    options.ColorScheme = ColorScheme.Dark;
}).AddEntityFramework();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;
        options.SuppressModelStateInvalidFilter = true;
    })
    .AddNewtonsoftJson(options => options.SerializerSettings.Configure());

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.WebHost.UseKestrel(options => options.AddServerHeader = false);
builder.WebHost.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
    options.ValidateOnBuild = true;
});

var app = builder.Build();
app.Logger.LogInformation("PublicApi App created...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

ValidatorOptions.Global.Configure();

var mapper = app.Services.GetRequiredService<IMapper>();
mapper.ConfigurationProvider.AssertConfigurationIsValid();
mapper.ConfigurationProvider.CompileMappings();

app.UseOpenApi(app.Services.GetRequiredService<IApiVersionDescriptionProvider>());
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        AllowCachingResponses = true,
        ResponseWriter = (context, healthReport) => context.Response.WriteAsync(healthReport.ToJson())
    });
app.UseHttpsRedirection();
app.UseHsts();
app.UseRouting();
app.UseHttpLogging();
app.UseResponseCompression();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiniProfiler();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.UseExceptionHandler(applicationBuilder =>
{
    applicationBuilder.Run(async context =>
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
});

app.Logger.LogInformation("Seeding Database...");
await app.MigrateDbContextAsync();

app.Logger.LogInformation("Launching PublicApi...");
await app.RunAsync();

#pragma warning disable CA1050
public partial class Program
{
}
#pragma warning restore CA1050