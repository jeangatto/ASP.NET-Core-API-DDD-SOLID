using FluentResults;
using FluentValidation.Results;
using SGP.Application.Requests.Common;
using System.Collections.Generic;
using System.Linq;

namespace SGP.Application.Extensions
{
    public static class BaseRequestExtensions
    {
        private const string AttemptedValue = "AttemptedValue";
        private const string ErrorCode = "ErrorCode";
        private const string PropertyName = "PropertyName";

        public static Result ToFail(this BaseRequest request)
        {
            return new Result().WithErrors(request.ValidationResult?.ToErrors());
        }

        public static Result<TResponse> ToFail<TResponse>(this BaseRequest request)
        {
            return new Result<TResponse>().WithErrors(request.ValidationResult?.ToErrors());
        }

        private static IEnumerable<Error> ToErrors(this ValidationResult validationResult)
        {
            return validationResult.IsValid ? Enumerable.Empty<Error>() : validationResult.Errors.ConvertAll(f =>
            {
                return new Error(f.ErrorMessage)
                    .WithMetadata(PropertyName, f.PropertyName)
                    .WithMetadata(AttemptedValue, f.AttemptedValue)
                    .WithMetadata(ErrorCode, f.ErrorCode);
            });
        }
    }
}
