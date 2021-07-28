using GraphQL.Types;
using SGP.Application.Responses;

namespace SGP.PublicApi.GraphQL.Types
{
    public sealed class CidadeType : ObjectGraphType<CidadeResponse>
    {
        public CidadeType()
        {
            Field(cidade => cidade.Regiao).Description("Nome da região");
            Field(cidade => cidade.Estado).Description("Nome do estado");
            Field(cidade => cidade.Uf).Description("Sigla da unidade federativa (UF)");
            Field(cidade => cidade.Nome).Description("Nome do município.");
            Field(cidade => cidade.Ibge).Description("Código único do município");
        }
    }
}