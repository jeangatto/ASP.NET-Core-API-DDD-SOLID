using FluentResults;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace SGP.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        public static Result ToFail(this ValidationResult validationResult)
            => new Result().WithErrors(validationResult.ToErrors());

        public static Result<T> ToFail<T>(this ValidationResult validationResult)
            => new Result<T>().WithErrors(validationResult.ToErrors());

        private static IEnumerable<Error> ToErrors(this ValidationResult validationResult)
        {
            return validationResult.IsValid ? Enumerable.Empty<Error>() : validationResult.Errors.ConvertAll(f =>
            {
                return new Error(f.ErrorMessage)
                    .WithMetadata(nameof(f.PropertyName), f.PropertyName)
                    .WithMetadata(nameof(f.AttemptedValue), f.AttemptedValue)
                    .WithMetadata(nameof(f.ErrorCode), f.ErrorCode);
            });
        }
    }
}