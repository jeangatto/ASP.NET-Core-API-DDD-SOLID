using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SGP.Shared.ContractResolvers;

namespace SGP.Shared.Extensions;

public static class JsonSettingsExtensions
{
    private static readonly CamelCaseNamingStrategy NamingStrategy = new();
    private static readonly StringEnumConverter StringEnumConverter = new(NamingStrategy);
    private static readonly PrivateSetterContractResolver ContractResolver = new(NamingStrategy);

    public static JsonSerializerSettings Configure(this JsonSerializerSettings settings)
    {
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
        settings.NullValueHandling = NullValueHandling.Ignore;
        settings.Formatting = Formatting.None;
        settings.ContractResolver = ContractResolver;
        settings.Converters.Add(StringEnumConverter);
        return settings;
    }
}