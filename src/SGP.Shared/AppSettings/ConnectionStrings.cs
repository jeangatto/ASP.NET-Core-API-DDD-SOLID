using SGP.Shared.Abstractions;

namespace SGP.Shared.AppSettings;

public sealed class ConnectionStrings : IAppOptions
{
    /// <summary>
    /// String de conexão com a base de dados relacional.
    /// </summary>
    public string Database { get; private init; } = string.Empty;

    /// <summary>
    /// (Opcional) Definição do Collation da base de dados relacional.
    /// REF: https://learn.microsoft.com/en-us/ef/core/miscellaneous/collations-and-case-sensitivity
    /// </summary>
    public string Collation { get; private init; } = string.Empty;

    /// <summary>
    /// String de conexão com o servidor de Cache.
    /// </summary>
    public string Cache { get; private init; } = string.Empty;
}