using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequest : BaseRequest<GetAllByEstadoRequest>
    {
        public GetAllByEstadoRequest(string estado)
        {
            Estado = estado;
        }

        public string Estado { get; }
    }
}