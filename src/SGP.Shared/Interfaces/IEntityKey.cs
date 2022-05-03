namespace SGP.Shared.Interfaces;

/// <summary>
/// Chave da Entidade.
/// </summary>
/// <typeparam name="TKey">O tipo da chave.</typeparam>
public interface IEntityKey<out TKey>
{
    /// <summary>
    /// Chave - Identificação Única (PK).
    /// </summary>
    TKey Id { get; }
}