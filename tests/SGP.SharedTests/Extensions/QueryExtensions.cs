using GraphQL.Query.Builder;
using SGP.SharedTests.GraphQL;

namespace SGP.SharedTests.Extensions
{
    public static class QueryExtensions
    {
        public static GraphQLRequest ToGraphQLRequest(this IQuery query)
            => new() { Query = "{" + query.Build() + "}" };
    }
}
