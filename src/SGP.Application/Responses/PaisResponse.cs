using SGP.Shared.Messages;
using System;
using System.Collections.Generic;

namespace SGP.Application.Responses
{
    public class PaisResponse : BaseResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public short Bacen { get; set; }

        public IEnumerable<EstadoResponse> Estados { get; set; }
    }
}