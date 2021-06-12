using GraphQL.Query.Builder;
using SGP.Shared.Messages;

namespace SGP.Tests.GraphQL
{
    public class QueryCamelCase<TResponse> : Query<TResponse> where TResponse : BaseResponse
    {
        public QueryCamelCase(string queryName)
            : base(queryName, new QueryOptions { Formatter = QueryFormatters.CamelCaseFormatter })
        {
        }
    }
}
