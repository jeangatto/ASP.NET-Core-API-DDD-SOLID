using Microsoft.Extensions.Configuration;
using SGP.Shared.Abstractions;

namespace SGP.Shared.Extensions;

public static class ConfigurationExtensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string configSectionPath)
        where TOptions : IAppOptions
    {
        return configuration
            .GetSection(configSectionPath)
            .Get<TOptions>(binderOptions => binderOptions.BindNonPublicProperties = true);
    }
}