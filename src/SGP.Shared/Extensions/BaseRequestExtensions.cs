using System.Collections.Generic;
using System.Linq;
using FluentResults;
using FluentValidation.Results;
using SGP.Shared.Errors;
using SGP.Shared.Messages;

namespace SGP.Shared.Extensions
{
    public static class BaseRequestExtensions
    {
        private static readonly IEnumerable<Error> EmptyErrors = Enumerable.Empty<Error>();

        public static Result ToFail(this BaseRequest request)
            => new Result().WithErrors(request.ValidationResult.ToErrors());

        public static Result<TResponse> ToFail<TResponse>(this BaseRequest request)
            => new Result<TResponse>().WithErrors(request.ValidationResult.ToErrors());

        private static IEnumerable<Error> ToErrors(this ValidationResult result)
            => result.IsValid ? EmptyErrors : result.Errors.ConvertAll(f => new ValidationError(f));
    }
}