using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterTodosPorUfRequestValidator : AbstractValidator<ObterTodosPorUfRequest>
    {
        public ObterTodosPorUfRequestValidator()
        {
            RuleFor(m => m.Uf)
                .NotEmpty()
                .Length(2);
        }
    }
}
