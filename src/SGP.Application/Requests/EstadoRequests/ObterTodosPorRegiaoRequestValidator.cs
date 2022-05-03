using FluentValidation;

namespace SGP.Application.Requests.EstadoRequests;

public class ObterTodosPorRegiaoRequestValidator : AbstractValidator<ObterTodosPorRegiaoRequest>
{
    public ObterTodosPorRegiaoRequestValidator()
        => RuleFor(x => x.Regiao).NotEmpty().MaximumLength(15);
}