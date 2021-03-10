using SGP.Domain.Enums;
using SGP.Shared.Messages;
using System;
using System.Collections.Generic;

namespace SGP.Application.Responses
{
    public class EstadoResponse : BaseResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public short Ibge { get; set; }
        public Regiao Regiao { get; set; }

        public IEnumerable<CidadeResponse> Cidades { get; set; }
    }
}