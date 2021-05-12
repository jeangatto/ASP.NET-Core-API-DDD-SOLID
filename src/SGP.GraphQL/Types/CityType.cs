using GraphQL.Types;
using SGP.Application.Responses;

namespace SGP.GraphQL.Types
{
    public class CityType : ObjectGraphType<CityResponse>
    {
        public CityType()
        {
            Field(c => c.Ibge)
                .Description("Código único do município");

            Field(c => c.Name)
                .Description("Nome do município.");

            Field(c => c.StateAbbr)
                .Description("Sigla da unidade federativa (UF)");
        }
    }
}