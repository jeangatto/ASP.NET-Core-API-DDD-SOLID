using GraphQL;
using GraphQL.Types;
using SGP.Domain.Repositories;
using SGP.Infrastructure.GraphQL.Types;

namespace SGP.Infrastructure.GraphQL.Querys
{
    public class CityQuery : ObjectGraphType
    {
        public CityQuery(ICityRepository repository)
        {
            FieldAsync<CityType>(
                name: "cityByIBGE",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "ibge",
                    Description = "IBGE da cidade"
                }),
                resolve: async context =>
                {
                    var ibge = context.GetArgument<string>("ibge");
                    return await repository.GetByIbgeAsync(ibge);
                });

            FieldAsync<ListGraphType<CityType>>(
                name: "citiesByStateAbbr",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>>
                {
                    Name = "stateAbbr",
                    Description = "Sigla da unidade federativa (UF)"
                }),
                resolve: async context =>
                {
                    var stateAbbr = context.GetArgument<string>("stateAbbr");
                    return await repository.GetAllCitiesAsync(stateAbbr);
                });
        }
    }
}