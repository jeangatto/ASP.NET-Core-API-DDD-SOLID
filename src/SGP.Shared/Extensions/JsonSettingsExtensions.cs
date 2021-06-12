using Ardalis.GuardClauses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SGP.Shared.ContractResolvers;

namespace SGP.Shared.Extensions
{
    public static class JsonSettingsExtensions
    {
        /// <summary>
        /// Configuração do serializador em JSON otimizado para gerar um JSON menor, resultando numa melhor performance.
        /// </summary>
        /// <param name="settings"></param>
        public static JsonSerializerSettings Configure(this JsonSerializerSettings settings)
        {
            Guard.Against.Null(settings, nameof(settings));

            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.Formatting = Formatting.None;

            var namingStrategy = new CamelCaseNamingStrategy();
            settings.ContractResolver = new PrivateSetterContractResolver
            {
                NamingStrategy = namingStrategy
            };
            settings.Converters.Add(new StringEnumConverter(namingStrategy));

            return settings;
        }
    }
}
