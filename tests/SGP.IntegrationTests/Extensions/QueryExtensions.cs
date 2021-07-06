using GraphQL.Query.Builder;
using SGP.IntegrationTests.Models;

namespace SGP.IntegrationTests.Extensions
{
    public static class QueryExtensions
    {
        public static GraphQLRequest ToGraphQLRequest(this IQuery query)
        {
            return new() { Query = "{" + query.Build() + "}" };
        }
    }
}