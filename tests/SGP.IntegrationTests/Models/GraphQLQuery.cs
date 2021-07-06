using GraphQL.Query.Builder;
using SGP.Shared.Messages;

namespace SGP.IntegrationTests.Models
{
    public class GraphQLQuery<TResponse> : Query<TResponse> where TResponse : BaseResponse
    {
        /// <summary>
        /// Inicializa uma nova instância do <see cref="Query{TSource}"/> com a
        /// <see cref="QueryOptions"/> configurado para o formato das propriedades em CamelCase.
        /// </summary>
        /// <param name="queryName">O nome da query do GraphQL.</param>
        public GraphQLQuery(string queryName)
            : base(queryName, new QueryOptions { Formatter = QueryFormatters.CamelCaseFormatter })
        {
        }
    }
}