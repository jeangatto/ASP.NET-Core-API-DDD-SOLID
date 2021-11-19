using GraphQL.Query.Builder;
using GraphQL.Server;

namespace SGP.Tests.Extensions
{
    public static class QueryExtensions
    {
        public static GraphQLRequest ToGraphQLRequest(this IQuery query)
            => new() { Query = "{" + query.Build() + "}" };
    }
}