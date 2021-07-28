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
        public static Result<T> ToExecutionError<T>(this Result<T> result, IResolveFieldContext<object> context)
        {
            foreach (var errorMessage in result.Errors.GroupByErrors())
            {
                context.Errors.Add(new ExecutionError(errorMessage));
            }

            return result;
        }

        public static ObjectResult ToHttpResult(this Result result)
        {
            if (result.IsFailed)
            {
                return result.ConvertErrorsToHttpNonSuccessResult();
            }

            return new OkObjectResult(new ApiResponse(StatusCodes.Status200OK));
        }

        public static ObjectResult ToHttpResult<T>(this Result<T> result)
        {
            if (result.IsFailed)
            {
                return result.ConvertErrorsToHttpNonSuccessResult();
            }

            return new OkObjectResult(new ApiResponse<T>(StatusCodes.Status200OK, result.Value));
        }

        private static ObjectResult ConvertErrorsToHttpNonSuccessResult(this ResultBase resultBase)
        {
            var errors = resultBase.Errors.GroupByErrors().Select(message => new ApiError(message));

            if (resultBase.HasError<NotFoundError>())
            {
                return new NotFoundObjectResult(
                    new ApiResponse(StatusCodes.Status404NotFound, errors));
            }

            return new BadRequestObjectResult(
                new ApiResponse(StatusCodes.Status400BadRequest, errors));
        }

        private static IEnumerable<string> GroupByErrors(this IEnumerable<Error> errors)
        {
            return errors
                .Select(error => error.Message)
                .Distinct()
                .OrderBy(message => message)
                .ToList();
        }
    }
}