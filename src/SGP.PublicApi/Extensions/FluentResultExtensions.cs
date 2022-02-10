using System.Collections.Generic;
using System.Linq;
using FluentResults;
using GraphQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Models;
using SGP.Shared.Errors;

namespace SGP.PublicApi.Extensions
{
    public static class FluentResultExtensions
    {
        private static readonly OkObjectResult EmptyOkResult = new(new ApiResponse(StatusCodes.Status200OK));

        public static void ToExecutionError<T>(this Result<T> result, IResolveFieldContext<object> context)
        {
            foreach (var errorMessage in result.Errors.GroupByErrors())
                context.Errors.Add(new ExecutionError(errorMessage));
        }

        public static ObjectResult ToHttpResult(this Result result)
            => result.IsFailed ? result.ToHttpNonSuccessResult() : EmptyOkResult;

        public static ObjectResult ToHttpResult<T>(this Result<T> result)
            => result.IsFailed
                ? result.ToHttpNonSuccessResult()
                : new OkObjectResult(new ApiResponse<T>(StatusCodes.Status200OK, result.Value));

        private static ObjectResult ToHttpNonSuccessResult(this ResultBase result)
        {
            var apiErrors = result.Errors.GroupByErrors().Select(message => new ApiError(message));

            if (result.HasError<NotFoundError>())
                return new NotFoundObjectResult(new ApiResponse(StatusCodes.Status404NotFound, apiErrors));

            return new BadRequestObjectResult(new ApiResponse(StatusCodes.Status400BadRequest, apiErrors));
        }

        private static IEnumerable<string> GroupByErrors(this IEnumerable<IError> errors)
            => errors.Select(error => error.Message).Distinct().OrderBy(message => message).ToList();
    }
}