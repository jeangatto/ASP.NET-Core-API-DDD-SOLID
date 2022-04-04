using System.Collections.Generic;
using SGP.Shared.Messages;

namespace SGP.PublicApi.Models;

public class ApiResponse : BaseResponse
{
    public ApiResponse(int statusCode) => StatusCode = statusCode;

    public ApiResponse(int statusCode, string message) : this(statusCode) => Errors = new[] {new ApiError(message)};

    public ApiResponse(int statusCode, ApiError apiError) : this(statusCode) => Errors = new[] {apiError};

    public ApiResponse(int statusCode, IEnumerable<ApiError> errors) : this(statusCode) => Errors = errors;

    public int StatusCode { get; }
    public IEnumerable<ApiError> Errors { get; }
}