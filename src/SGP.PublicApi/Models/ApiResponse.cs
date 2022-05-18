using System.Collections.Generic;
using SGP.Shared.Messages;

namespace SGP.PublicApi.Models;

public class ApiResponse : BaseResponse
{
    public ApiResponse(bool success, int statusCode)
    {
        Success = success;
        StatusCode = statusCode;
    }

    public ApiResponse(bool success, int statusCode, string message) : this(success, statusCode)
    {
        Errors = new[] { new ApiError(message) };
    }

    public ApiResponse(bool success, int statusCode, ApiError apiError) : this(success, statusCode)
    {
        Errors = new[] { apiError };
    }

    public ApiResponse(bool success, int statusCode, IEnumerable<ApiError> errors) : this(success, statusCode)
    {
        Errors = errors;
    }

    public bool Success { get; }
    public int StatusCode { get; }
    public IEnumerable<ApiError> Errors { get; }
}