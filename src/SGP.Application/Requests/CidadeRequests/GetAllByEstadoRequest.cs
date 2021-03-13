using FluentValidation;
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
            var validator = new InlineValidator<GetAllByEstadoRequest>();
            validator.RuleFor(x => x.Estado).NotEmpty().Length(2);
            validator.Validate(this).AddToNotifiable(this);
        }
    }
}
