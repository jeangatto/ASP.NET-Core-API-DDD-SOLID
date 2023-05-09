using System.Threading.Tasks;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace SGP.Shared.Messages;

/// <inheritdoc />
public abstract class BaseRequestWithValidation : IRequest
{
    protected BaseRequestWithValidation() => ValidationResult = new ValidationResult();

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