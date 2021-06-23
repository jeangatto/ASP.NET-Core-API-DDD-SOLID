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
        public static Result<T> ToExecutionError<T>(this Result<T> result,
            IResolveFieldContext<object> context)
        {
            foreach (var error in result.Errors)
            {
                context.Errors.Add(new ExecutionError(error.Message));
            }

            return result;
        }

        public static ObjectResult ToHttpResult<T>(this Result<T> result)
        {
            if (result.IsFailed)
            {
                var errors = result.Errors.ConvertAll(err => new ApiError(err.Message));

                if (result.HasError<NotFoundError>())
                {
                    return new NotFoundObjectResult(
                        new ApiResponse<T>(StatusCodes.Status404NotFound, errors));
                }

                return new BadRequestObjectResult(
                    new ApiResponse<T>(StatusCodes.Status400BadRequest, errors));
            }

            return new OkObjectResult(new ApiResponse<T>(StatusCodes.Status200OK, result.Value));
        }
    }
}