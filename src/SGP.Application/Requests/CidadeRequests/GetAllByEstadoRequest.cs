using SGP.Shared.Extensions;
using SGP.Shared.Messages;
using SGP.Shared.Utils;

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
            FluentValidationUtils.GetValidatorInstance<GetAllByEstadoRequest>(true)?
                .Validate(this)
                .AddToNotifiable(this);
        }
    }
}