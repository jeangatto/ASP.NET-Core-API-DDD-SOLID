using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests;

public class ObterPorIbgeRequestValidator : AbstractValidator<ObterPorIbgeRequest>
{
    public ObterPorIbgeRequestValidator()
        => RuleFor(x => x.Ibge).NotEmpty().GreaterThan(0);
}