using FluentResults;
using SGP.Application.Configuration.Queries;
using SGP.Application.Responses;
using System.Collections.Generic;

namespace SGP.Application.Queries.Cidades
{
    public class ObterCidadesPorUfQuery : IQuery<Result<IEnumerable<CidadeResponse>>>
    {
        public string Uf { get; }

        public ObterCidadesPorUfQuery(string uf) => Uf = uf;

        public override string ToString() => Uf;
    }
}