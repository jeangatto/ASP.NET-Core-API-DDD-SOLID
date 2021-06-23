using GraphQL.Query.Builder;
using SGP.SharedTests.GraphQL;

namespace SGP.SharedTests.Extensions
{
    public static class QueryExtensions
    {
        public static GraphQLRequest ToGraphQLRequest(this IQuery query)
        {
            return new() { Query = "{" + query.Build() + "}" };
        }
    }
}