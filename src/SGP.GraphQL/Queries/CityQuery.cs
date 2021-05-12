using GraphQL;
using GraphQL.Types;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CityRequests;
using SGP.Application.Responses;
using SGP.GraphQL.Extensions;
using SGP.GraphQL.Types;
using System.Linq;

namespace SGP.GraphQL.Queries
{
    public class CityQuery : ObjectGraphType
    {
        public CityQuery(ICityService service)
        {
            FieldAsync<CityType>(
                name: "cityByIBGE",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>>
                    {
                        Name = "ibge",
                        Description = "IBGE da cidade"
                    }),
                resolve: async context =>
                {
                    var ibge = context.GetArgument<string>("ibge");
                    var request = new GetByIbgeRequest(ibge);

                    var result = await service.GetByIbgeAsync(request);
                    if (result.IsFailed)
                    {
                        result.ToExecutionError(context);
                        return null;
                    }

                    return result.Value;
                });

            FieldAsync<ListGraphType<CityType>>(
                name: "citiesByState",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>>
                    {
                        Name = "state",
                        Description = "Sigla da unidade federativa (UF)"
                    }),
                resolve: async context =>
                {
                    var state = context.GetArgument<string>("state");
                    var request = new GetAllByStateRequest(state);

                    var result = await service.GetAllCitiesAsync(request);
                    if (result.IsFailed)
                    {
                        result.ToExecutionError(context);
                        return Enumerable.Empty<CityResponse>();
                    }

                    return result.Value;
                });
        }
    }
}