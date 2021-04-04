using FluentValidation.Results;
using SGP.Shared.Results;

namespace SGP.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        public static IResult ToResult(this ValidationResult validationResult)
        {
            if (validationResult?.IsValid == false)
            {
                return Result.Failure(validationResult.ToString());
            }

            return Result.Success();
        }

        public static IResult<T> ToResult<T>(this ValidationResult validationResult) where T : class
        {
            if (validationResult?.IsValid == false)
            {
                return Result.Failure<T>(validationResult.ToString());
            }

            return Result.Success<T>();
        }
    }
}