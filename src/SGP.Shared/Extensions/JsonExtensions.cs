using Newtonsoft.Json;

namespace SGP.Shared.Extensions
{
    /// <summary>
    /// Extensões para a utilização do JSON.
    /// </summary>
    public static class JsonExtensions
    {
        private static readonly JsonSerializerSettings Settings
            = new JsonSerializerSettings().Configure();

        /// <summary>
        /// Desserializa o JSON para o tipo especificado.
        /// </summary>
        /// <typeparam name="T">O tipo de objeto para o qual desserializar.</typeparam>
        /// <param name="value">O objeto a ser desserializado.</param>
        /// <returns>O objeto desserializado da string JSON.</returns>
        public static T FromJson<T>(this string value)
            => JsonConvert.DeserializeObject<T>(value, Settings);

        /// <summary>
        /// Serializa o objeto especificado em uma string JSON.
        /// </summary>
        /// <param name="value">O objeto a ser serializado.</param>
        /// <returns>Uma representação de string JSON do objeto.</returns>
        public static string ToJson<T>(this T value)
            => JsonConvert.SerializeObject(value, Settings);
    }
}