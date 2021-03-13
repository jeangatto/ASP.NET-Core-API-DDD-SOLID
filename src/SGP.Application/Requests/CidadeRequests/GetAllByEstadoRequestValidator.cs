using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetAllByEstadoRequestValidator : AbstractValidator<GetAllByEstadoRequest>
    {
        public GetAllByEstadoRequestValidator()
        {
            RuleFor(x => x.Estado)
                .NotEmpty()
                .Length(2);
        }
    }
}
