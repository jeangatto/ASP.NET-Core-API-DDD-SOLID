using FluentResults;
using FluentValidation.Results;

namespace SGP.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        public static Result ToFail(this ValidationResult validationResult)
        {
            var reasons = validationResult.Errors.ConvertAll(f => new Error(f.ErrorMessage));
            var error = new Error().CausedBy(reasons);
            return Result.Fail(error);
        }

        public static Result<T> ToFail<T>(this ValidationResult validationResult)
        {
            var reasons = validationResult.Errors.ConvertAll(f => new Error(f.ErrorMessage));
            var error = new Error().CausedBy(reasons);
            return Result.Fail<T>(error);
        }
    }
}