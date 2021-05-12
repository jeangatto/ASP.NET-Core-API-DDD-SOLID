using FluentResults;
using FluentValidation.Results;

namespace SGP.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        public static Result ToFail(this ValidationResult validationResult)
        {
            var errors = validationResult.Errors.ConvertAll(f
                => new Error(f.ErrorMessage));

            return new Result().WithErrors(errors);
        }

        public static Result<T> ToFail<T>(this ValidationResult validationResult)
        {
            var errors = validationResult.Errors.ConvertAll(f
                => new Error(f.ErrorMessage));

            return new Result<T>().WithErrors(errors);
        }
    }
}