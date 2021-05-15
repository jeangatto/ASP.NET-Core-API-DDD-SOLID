using System.Collections.Generic;

namespace SGP.Tests.Models
{
    public class GraphQLResponse<T>
    {
        public T Data { get; set; }
        public IEnumerable<GraphQLErrorResponse> Errors { get; set; }
    }
}
