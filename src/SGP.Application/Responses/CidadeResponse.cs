using SGP.Shared.Messages;

namespace SGP.Application.Responses
{
    public class CidadeResponse : BaseResponse
    {
        public string Ibge { get; set; }
        public string Estado { get; set; }
        public string Nome { get; set; }
    }
}