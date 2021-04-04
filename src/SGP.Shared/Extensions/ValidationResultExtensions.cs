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

        public static IResult<TResponse> ToResult<TResponse>(this ValidationResult validationResult)
            where TResponse : class
        {
            if (validationResult?.IsValid == false)
            {
                return Result.Failure<TResponse>(validationResult.ToString());
            }

            return Result.Success<TResponse>();
        }
    }
}