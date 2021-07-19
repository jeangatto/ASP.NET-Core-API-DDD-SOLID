using GraphQL;
using GraphQL.Types;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.PublicApi.Extensions;
using SGP.PublicApi.GraphQL.Constants;
using SGP.PublicApi.GraphQL.Types;
using System.Linq;

namespace SGP.PublicApi.GraphQL.Queries
{
    public class CidadeQuery : ObjectGraphType
    {
        public CidadeQuery(ICidadeService service)
        {
            FieldAsync<CidadeType>(
                name: QueryNames.CidadePorIbge,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>>
                {
                    Name = "ibge",
                    Description = "IBGE da cidade"
                }),
                resolve: async context =>
                {
                    var request = new ObterPorIbgeRequest(context.GetArgument<int>("ibge"));

                    var result = await service.ObterPorIbgeAsync(request);
                    if (result.IsFailed)
                    {
                        result.ToExecutionError(context);
                        return null;
                    }

                    return result.Value;
                });

            FieldAsync<ListGraphType<CidadeType>>(
                name: QueryNames.CidadesPorEstado,
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "uf",
                    Description = "Sigla da unidade federativa (UF)"
                }),
                resolve: async context =>
                {
                    var request = new ObterTodosPorUfRequest(context.GetArgument<string>("uf"));

                    var result = await service.ObterTodosPorUfAsync(request);
                    if (result.IsFailed)
                    {
                        result.ToExecutionError(context);
                        return Enumerable.Empty<CidadeType>();
                    }

                    return result.Value;
                });
        }
    }
}