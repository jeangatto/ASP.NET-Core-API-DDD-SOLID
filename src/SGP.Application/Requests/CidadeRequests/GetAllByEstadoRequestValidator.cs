using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequestValidator : AbstractValidator<GetAllByEstadoRequest>
    {
        public GetAllByEstadoRequestValidator()
        {
            RuleFor(request => request.EstadoSigla)
                .NotEmpty()
                .Length(2);
        }
    }
}
