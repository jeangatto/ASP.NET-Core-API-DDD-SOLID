using Microsoft.AspNetCore.Mvc;
using SGP.PublicApi.Models;

namespace SGP.Tests.Extensions;

public static class ActionResultExtensions
{
    public static ApiResponse ToApiResponse(this IActionResult actionResult)
    {
        var objectResult = (ObjectResult)actionResult;
        return (ApiResponse)objectResult.Value;
    }

    public static ApiResponse<T> ToApiResponse<T>(this IActionResult actionResult)
    {
        var objectResult = (ObjectResult)actionResult;
        return (ApiResponse<T>)objectResult.Value;
    }
}