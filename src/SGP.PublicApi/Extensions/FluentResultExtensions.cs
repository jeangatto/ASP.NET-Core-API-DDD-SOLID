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
    private static readonly OkObjectResult EmptyOkResult = new(new ApiResponse(true, StatusCodes.Status200OK));

    public static ObjectResult ToHttpResult(this Result result)
        => result.IsFailed ? result.ToHttpNonSuccessResult() : EmptyOkResult;

    public static ObjectResult ToHttpResult<T>(this Result<T> result)
        => result.IsFailed
            ? result.ToHttpNonSuccessResult()
            : new OkObjectResult(new ApiResponse<T>(true, StatusCodes.Status200OK, result.Value));

    private static ObjectResult ToHttpNonSuccessResult(this ResultBase result)
    {
        var errors = result.Errors.GroupByErrors().Select(message => new ApiError(message));

        if (result.HasError<NotFoundError>())
            return new NotFoundObjectResult(new ApiResponse(false, StatusCodes.Status404NotFound, errors));

        return new BadRequestObjectResult(new ApiResponse(false, StatusCodes.Status400BadRequest, errors));
    }

    private static IEnumerable<string> GroupByErrors(this IEnumerable<IError> errors)
        => errors
            .Select(error => error.Message)
            .Distinct()
            .OrderBy(message => message)
            .ToList();
}