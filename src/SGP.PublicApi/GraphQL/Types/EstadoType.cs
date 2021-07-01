using GraphQL.Types;
using SGP.Application.Responses;

namespace SGP.PublicApi.GraphQL.Types
{
    public class EstadoType : ObjectGraphType<EstadoResponse>
    {
        public EstadoType()
        {
            Field(cidade => cidade.Regiao).Description("Nome da região");
            Field(cidade => cidade.Uf).Description("Sigla da unidade federativa (UF)");
            Field(cidade => cidade.Nome).Description("Nome do estado.");
        }
    }
}