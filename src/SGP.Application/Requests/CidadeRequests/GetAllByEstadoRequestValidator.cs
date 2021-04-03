using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequestValidator : AbstractValidator<GetAllByEstadoRequest>
    {
        public GetAllByEstadoRequestValidator()
        {
            RuleFor(req => req.EstadoSigla)
                .NotEmpty()
                .Length(2);
        }
    }
}
