using SGP.Application.Responses.Common;

namespace SGP.Application.Responses
{
    public class CidadeResponse : BaseResponse
    {
        public string Ibge { get; set; }
        public string Estado { get; set; }
        public string Nome { get; set; }
    }
}