using Newtonsoft.Json;

namespace SGP.Infrastructure.Extensions
{
    /// <summary>
    /// Extensões para a utilização do JSON.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Configuração padrão do serializador em JSON.
        /// Otimizado para gerar um JSON menor, resultando numa melhor performance.
        /// </summary>
        public static readonly JsonSerializerSettings SerializerSettings = new()
        {
            // Formatando o JSON em uma única linha.
            Formatting = Formatting.None,
            // Removendo as referências circulares.
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            // Ignorando as propriedades que estão com valores nulo.
            NullValueHandling = NullValueHandling.Ignore
        };

        /// <summary>
        /// Desserializa o JSON para o tipo especificado.
        /// </summary>
        /// <typeparam name="T">O tipo de objeto para o qual desserializar.</typeparam>
        /// <param name="value">O objeto a ser desserializado.</param>
        /// <returns>O objeto desserializado da string JSON.</returns>
        public static T FromJson<T>(this string value)
            => JsonConvert.DeserializeObject<T>(value, SerializerSettings);

        /// <summary>
        /// Serializa o objeto especificado em uma string JSON.
        /// </summary>
        /// <param name="value">O objeto a ser serializado.</param>
        /// <returns>Uma representação de string JSON do objeto.</returns>
        public static string ToJson(this object value)
            => JsonConvert.SerializeObject(value, SerializerSettings);
    }
}