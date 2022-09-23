using System.Collections.Generic;
using System.Linq;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Models;
using SGP.Shared.Errors;

namespace SGP.PublicApi.Extensions;

public static class FluentResultExtensions
{
    private static readonly OkObjectResult EmptyOkResult = new(ApiResponse.Ok());

    public static ObjectResult ToHttpResult(this Result result)
        => result.IsFailed ? result.ToHttpNonSuccessResult() : EmptyOkResult;

    public static ObjectResult ToHttpResult<T>(this Result<T> result)
        => result.IsFailed ? result.ToHttpNonSuccessResult() : new OkObjectResult(ApiResponse<T>.Ok(result.Value));

    private static ObjectResult ToHttpNonSuccessResult(this ResultBase result)
    {
        var errors = result.Errors.ToApiErrors();

        if (result.HasError<ValidationError>() || result.HasError<BusinessError>())
            return new BadRequestObjectResult(ApiResponse.BadRequest(errors));

        if (result.HasError<NotFoundError>())
            return new NotFoundObjectResult(ApiResponse.NotFound(errors));

        if (result.HasError<UnauthorizedError>())
        {
            return new ObjectResult(ApiResponse.Unauthorized(errors))
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }

        return new ObjectResult(ApiResponse.InternalServerError(errors))
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }

    private static IEnumerable<ApiError> ToApiErrors(this IEnumerable<IError> errors)
        => errors
            .Select(error => error.Message)
            .Distinct()
            .OrderBy(message => message)
            .Select(message => new ApiError(message))
            .ToList();
}