using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequestValidator : AbstractValidator<GetAllByEstadoRequest>
    {
        public GetAllByEstadoRequestValidator()
        {
            RuleFor(x => x.EstadoSigla)
                .NotEmpty()
                .Length(2);
        }
    }
}
