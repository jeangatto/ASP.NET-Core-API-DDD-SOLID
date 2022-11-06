using System.Linq;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Models;
using SGP.PublicApi.ObjectResults;

namespace SGP.PublicApi.Extensions;

public static class ResultExtensions
{
    private static readonly OkObjectResult EmptyOkResult = new(ApiResponse.Ok());

    public static IActionResult ToActionResult(this Result result)
        => result.IsSuccess ? EmptyOkResult : result.ToHttpNonSuccessResult();

    public static IActionResult ToActionResult<T>(this Result<T> result)
        => result.IsSuccess ? new OkObjectResult(ApiResponse<T>.Ok(result.Value)) : result.ToHttpNonSuccessResult();

    private static IActionResult ToHttpNonSuccessResult(this IResult result)
    {
        if (result.Status == ResultStatus.Error)
        {
            return new BadRequestObjectResult(
                ApiResponse.BadRequest(result.Errors.Select(error => new ApiError(error)).ToList()));
        }
        else if (result.Status == ResultStatus.Invalid)
        {
            return new BadRequestObjectResult(
                ApiResponse.BadRequest(result.ValidationErrors.ConvertAll(e => new ApiError(e.ErrorMessage))));
        }
        else if (result.Status == ResultStatus.NotFound)
        {
            return new NotFoundObjectResult(
                ApiResponse.NotFound(result.Errors.Select(error => new ApiError(error)).ToList()));
        }
        else if (result.Status == ResultStatus.Unauthorized)
        {
            return new UnauthorizedObjectResult(
                ApiResponse.Unauthorized(result.Errors.Select(error => new ApiError(error)).ToList()));
        }
        else if (result.Status == ResultStatus.Forbidden)
        {
            return new ForbiddenObjectResult(
                ApiResponse.Forbidden(result.Errors.Select(error => new ApiError(error)).ToList()));
        }

        return new InternalServerErrorObjectResult(
            ApiResponse.InternalServerError(result.Errors.Select(error => new ApiError(error)).ToList()));
    }
}