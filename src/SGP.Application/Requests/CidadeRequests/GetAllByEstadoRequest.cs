using SGP.Application.Requests.Common;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequest : BaseRequest
    {
        public GetAllByEstadoRequest(string estadoSigla)
        {
            EstadoSigla = estadoSigla;
        }

        public string EstadoSigla { get; }
    }
}