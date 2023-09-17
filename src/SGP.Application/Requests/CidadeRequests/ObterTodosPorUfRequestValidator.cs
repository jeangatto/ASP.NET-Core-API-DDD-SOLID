using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests;

public class ObterTodosPorUfRequestValidator : AbstractValidator<ObterTodosPorUfRequest>
{
    public ObterTodosPorUfRequestValidator() =>
        RuleFor(req => req.Uf).NotEmpty().Length(2);
}
