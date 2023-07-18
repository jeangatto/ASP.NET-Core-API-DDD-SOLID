using System.Linq;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Models;
using SGP.PublicApi.ObjectResults;

namespace SGP.PublicApi.Extensions;

public static class ResultExtensions
{
    private static readonly OkObjectResult EmptyOkResult = new(ApiResponse.Ok());

    public static IActionResult ToActionResult(this Result result) =>
        result.IsSuccess
            ? EmptyOkResult
            : result.ToHttpNonSuccessResult();

    public static IActionResult ToActionResult<T>(this Result<T> result) =>
        result.IsSuccess
            ? new OkObjectResult(ApiResponse<T>.Ok(result.Value))
            : result.ToHttpNonSuccessResult();

    private static IActionResult ToHttpNonSuccessResult(this IResult result)
    {
        var errors = result.Errors.Select(error => new ApiError(error)).ToList();

        switch (result.Status)
        {
            case ResultStatus.Invalid:

                var validationErrors = result.ValidationErrors.ConvertAll(e => new ApiError(e.ErrorMessage));
                return new BadRequestObjectResult(ApiResponse.BadRequest(validationErrors));

            case ResultStatus.NotFound:
                return new NotFoundObjectResult(ApiResponse.NotFound(errors));

            case ResultStatus.Unauthorized:
                return new UnauthorizedObjectResult(ApiResponse.Unauthorized(errors));

            case ResultStatus.Forbidden:
                return new ForbiddenObjectResult(ApiResponse.Forbidden(errors));

            default:
                return new BadRequestObjectResult(ApiResponse.BadRequest(errors));
        }
    }
}