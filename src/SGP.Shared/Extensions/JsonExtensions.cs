using System;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SGP.Shared.Extensions;

/// <summary>
/// Extensões para a utilização do JSON.
/// </summary>
public static class JsonExtensions
{
    private static readonly CamelCaseNamingStrategy NamingStrategy = new();
    private static readonly StringEnumConverter EnumConverter = new(NamingStrategy);
    private static readonly PrivateSetterContractResolver ContractResolver = new() { NamingStrategy = NamingStrategy };

    private static readonly Lazy<JsonSerializerSettings> LazySettings =
        new(() => new JsonSerializerSettings().Configure(), isThreadSafe: true);

    /// <summary>
    /// Desserializa o JSON para o tipo especificado.
    /// </summary>
    /// <typeparam name="T">O tipo de objeto para o qual desserializar.</typeparam>
    /// <param name="value">O objeto a ser desserializado.</param>
    /// <returns>O objeto desserializado da string JSON.</returns>
    public static T FromJson<T>(this string value) =>
        value != null ? JsonConvert.DeserializeObject<T>(value, LazySettings.Value) : default;

    /// <summary>
    /// Serializa o objeto especificado em uma string JSON.
    /// </summary>
    /// <param name="value">O objeto a ser serializado.</param>
    /// <returns>Uma representação de string JSON do objeto.</returns>
    public static string ToJson<T>(this T value) =>
        !value.IsDefault() ? JsonConvert.SerializeObject(value, LazySettings.Value) : default;

    public static JsonSerializerSettings Configure(this JsonSerializerSettings settings)
    {
        settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        settings.PreserveReferencesHandling = PreserveReferencesHandling.None;
        settings.NullValueHandling = NullValueHandling.Ignore;
        settings.Formatting = Formatting.None;
        settings.ContractResolver = ContractResolver;
        settings.Converters.Add(EnumConverter);
        return settings;
    }
}