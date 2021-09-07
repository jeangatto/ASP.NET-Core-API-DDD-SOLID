using Microsoft.Extensions.Configuration;

namespace SGP.Shared.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetWithNonPublicProperties<T>(this IConfiguration configuration)
        {
            return configuration.GetSection(typeof(T).Name).Get<T>(options => options.BindNonPublicProperties = true);
        }
    }
}