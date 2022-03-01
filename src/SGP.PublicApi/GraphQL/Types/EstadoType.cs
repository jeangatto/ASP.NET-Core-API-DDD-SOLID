using GraphQL.Types;
using SGP.Application.Responses;

namespace SGP.PublicApi.GraphQL.Types
{
    public sealed class EstadoType : ObjectGraphType<EstadoResponse>
    {
        public EstadoType()
        {
            Field(estado => estado.Regiao).Description("Nome da região");
            Field(estado => estado.Uf).Description("Sigla da unidade federativa (UF)");
            Field(estado => estado.Nome).Description("Nome do estado.");
        }
    }
}