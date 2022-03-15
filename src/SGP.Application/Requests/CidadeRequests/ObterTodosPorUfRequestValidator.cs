using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class ObterTodosPorUfRequestValidator : AbstractValidator<ObterTodosPorUfRequest>
    {
        public ObterTodosPorUfRequestValidator()
            => RuleFor(x => x.Uf).NotEmpty().Length(2);
    }
}