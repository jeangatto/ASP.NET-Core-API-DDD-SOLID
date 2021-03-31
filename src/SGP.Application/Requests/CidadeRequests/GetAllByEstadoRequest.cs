using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequest : BaseRequest
    {
        public GetAllByEstadoRequest(string estadoSigla)
        {
            EstadoSigla = estadoSigla;
        }

        public string EstadoSigla { get; }

        public override void Validate()
        {
            AddNotifications(new GetAllByEstadoRequestValidator().Validate(this));
        }
    }
}