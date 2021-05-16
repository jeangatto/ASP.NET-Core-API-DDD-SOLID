using System.Collections.Generic;

namespace SGP.Tests.Models
{
    public class GraphQLResponse<T>
    {
        public GraphQLResponse(T data, IEnumerable<GraphQLErrorResponse> errors)
        {
            Data = data;
            Errors = errors;
        }

        public T Data { get; }
        public IEnumerable<GraphQLErrorResponse> Errors { get; }
    }
}
