using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterPorIbgeRequestValidator : AbstractValidator<ObterPorIbgeRequest>
    {
        public ObterPorIbgeRequestValidator()
        {
            RuleFor(m => m.Ibge)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}