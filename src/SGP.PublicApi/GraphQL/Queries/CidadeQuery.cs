namespace SGP.PublicApi.GraphQL.Queries
{
    using Application.Interfaces;
    using Application.Requests.CidadeRequests;
    using Constants;
    using Extensions;
    using global::GraphQL;
    using global::GraphQL.Types;
    using System.Linq;
    using Types;

    public class CidadeQuery : ObjectGraphType
    {
        public CidadeQuery(ICidadeService service)
        {
            FieldAsync<CidadeType>(
                QueryNames.CidadePorIbge,
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
                QueryNames.CidadesPorEstado,
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