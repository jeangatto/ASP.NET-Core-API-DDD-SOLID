using Microsoft.Extensions.Configuration;
using SGP.Shared.Abstractions;

namespace SGP.Shared.Extensions;

public static class ConfigurationExtensions
{
    public static TOptions GetOptions<TOptions>(this IConfiguration configuration)
        where TOptions : class, IAppOptions
    {
        return configuration
            .GetRequiredSection(TOptions.ConfigSectionPath)
            .Get<TOptions>(binderOptions => binderOptions.BindNonPublicProperties = true);
    }
}
