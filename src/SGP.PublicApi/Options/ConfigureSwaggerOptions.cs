using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SGP.PublicApi.Options;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var openApiInfo = new OpenApiInfo
        {
            Title = "Sistema Gerenciador de Pedidos (SGP)",
            Description = "ASP.NET Core C# REST API, DDD, Princípios SOLID e Clean Architecture",
            Version = description.ApiVersion.ToString(),
            Contact = new OpenApiContact { Name = "Jean Gatto", Email = "jean_gatto@hotmail.com" }
        };

        if (description.IsDeprecated)
            openApiInfo.Description += " - Esta versão da API foi descontinuada.";

        return openApiInfo;
    }
}