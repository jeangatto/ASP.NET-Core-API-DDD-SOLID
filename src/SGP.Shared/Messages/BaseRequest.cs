using System.Threading.Tasks;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace SGP.Shared.Messages;

/// <summary>
/// Classe base usada por solicitações da API.
/// </summary>
public abstract class BaseRequest
{
    protected BaseRequest() => ValidationResult = new ValidationResult();

    [JsonIgnore]
    public ValidationResult ValidationResult { get; protected set; }

    /// <summary>
    /// Indica se a requisição é valida.
    /// </summary>
    [JsonIgnore]
    public bool IsValid => ValidationResult.IsValid;

    /// <summary>
    /// Valida a requisição.
    /// </summary>
    public abstract Task ValidateAsync();
}