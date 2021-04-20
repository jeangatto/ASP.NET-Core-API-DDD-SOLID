using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SGP.Shared.ContractResolvers;

namespace SGP.Shared.Extensions
{
    /// <summary>
    /// Extensões para a utilização do JSON.
    /// </summary>
    public static class JsonExtensions
    {
        private static readonly NamingStrategy DefaultNamingStrategy = new CamelCaseNamingStrategy();

        /// <summary>
        /// Configuração padrão do serializador em JSON.
        /// Otimizado para gerar um JSON menor, resultando numa melhor performance.
        /// </summary>
        private static readonly JsonSerializerSettings JsonOptions = new()
        {
            Formatting = Formatting.None,
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new PrivateSetterContractResolver(DefaultNamingStrategy),
            Converters = new[] { new StringEnumConverter(DefaultNamingStrategy) }
        };

        /// <summary>
        /// Desserializa o JSON para o tipo especificado.
        /// </summary>
        /// <typeparam name="T">O tipo de objeto para o qual desserializar.</typeparam>
        /// <param name="jsonString">O objeto a ser desserializado.</param>
        /// <returns>O objeto desserializado da string JSON.</returns>
        public static T FromJson<T>(this string jsonString)
            => JsonConvert.DeserializeObject<T>(jsonString, JsonOptions);

        /// <summary>
        /// Serializa o objeto especificado em uma string JSON.
        /// </summary>
        /// <param name="value">O objeto a ser serializado.</param>
        /// <returns>Uma representação de string JSON do objeto.</returns>
        public static string ToJson<T>(this T value)
            => JsonConvert.SerializeObject(value, JsonOptions);
    }
}