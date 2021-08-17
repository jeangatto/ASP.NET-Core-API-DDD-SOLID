using System.Collections.Generic;

namespace SGP.PublicApi.Models
{
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(int statusCode, T result) : base(statusCode)
        {
            Result = result;
        }

        public ApiResponse(int statusCode, IEnumerable<ApiError> errors) : base(statusCode, errors)
        {
        }

        public ApiResponse(int statusCode, ApiError apiError) : base(statusCode, apiError)
        {
        }

        public T Result { get; }
    }
}