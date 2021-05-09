using GraphQL.Types;
using SGP.Domain.Entities;

namespace SGP.Infrastructure.GraphQL.Types
{
    public class CityType : ObjectGraphType<City>
    {
        public CityType()
        {
            Field(c => c.Ibge);
            Field(c => c.Name).Description("Nome do município.");
            Field(c => c.StateAbbr).Description("Sigla da unidade federativa (UF)");
        }
    }
}