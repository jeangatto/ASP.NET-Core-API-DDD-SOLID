using SGP.Shared.Extensions;
using SGP.Shared.Messages;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequest : BaseRequest
    {
        public GetAllByEstadoRequest(string estado)
        {
            Estado = estado;
        }

        public string Estado { get; }

        public override void Validate()
        {
            new GetAllByEstadoRequestValidator()
                .Validate(this)
                .AddToNotifiable(this);
        }
    }
}