using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequestValidator : AbstractValidator<GetByIbgeRequest>
    {
        public GetByIbgeRequestValidator()
        {
            RuleFor(x => x.Ibge)
                .NotEmpty()
                .MaximumLength(8);
        }
    }
}
