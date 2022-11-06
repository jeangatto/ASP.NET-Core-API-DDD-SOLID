using FluentValidation;

namespace SGP.Application.Requests.EstadoRequests;

public class ObterTodosPorRegiaoRequestValidator : AbstractValidator<ObterTodosPorRegiaoRequest>
{
    public ObterTodosPorRegiaoRequestValidator()
        => RuleFor(req => req.Regiao).NotEmpty().MaximumLength(15);
}