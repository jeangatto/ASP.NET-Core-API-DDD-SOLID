using SGP.Shared.Messages;
using System.Collections.Generic;

namespace SGP.PublicApi.Models
{
    public class ApiResponse : BaseResponse
    {
        public ApiResponse(int statusCode)
        {
            StatusCode = statusCode;
        }

        public ApiResponse(int statusCode, IEnumerable<ApiError> errors)
            : this(statusCode)
        {
            Errors = errors;
        }

        public ApiResponse(int statusCode, ApiError apiError)
            : this(statusCode)
        {
            Errors = new[] { apiError };
        }

        public int StatusCode { get; private set; }
        public IEnumerable<ApiError> Errors { get; private set; }
    }
}