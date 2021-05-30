using FluentResults;
using SGP.Application.Configuration.Queries;
using SGP.Application.Responses;

namespace SGP.Application.Queries.Cidades
{
    public class ObterCidadePorIbgeQuery : IQuery<Result<CidadeResponse>>
    {
        public int Ibge { get; }

        public ObterCidadePorIbgeQuery(int ibge) => Ibge = ibge;

        public override string ToString() => Ibge.ToString();
    }
}