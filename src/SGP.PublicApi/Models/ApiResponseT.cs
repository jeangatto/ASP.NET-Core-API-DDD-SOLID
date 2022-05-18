using System.Collections.Generic;

namespace SGP.PublicApi.Models;

public class ApiResponse<T> : ApiResponse
{
    public ApiResponse(bool success, int statusCode, T result) : base(success, statusCode)
    {
        Result = result;
    }

    public ApiResponse(bool success, int statusCode, IEnumerable<ApiError> errors) : base(success, statusCode, errors)
    {
    }

    public ApiResponse(bool success, int statusCode, ApiError apiError) : base(success, statusCode, apiError)
    {
    }

    public T Result { get; }
}