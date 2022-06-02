using FluentResults;
using FluentValidation.Results;

namespace SGP.Shared.Errors;

public sealed class ValidationError : Error
{
    public ValidationError(string message) : base(message)
    {
    }

    public ValidationError(ValidationFailure failure) : base(failure.ErrorMessage)
    {
    }
}