using SGP.Shared.Messages;
using System;

namespace SGP.Application.Responses
{
    public class CidadeResponse : BaseResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public int Ibge { get; set; }
    }
}