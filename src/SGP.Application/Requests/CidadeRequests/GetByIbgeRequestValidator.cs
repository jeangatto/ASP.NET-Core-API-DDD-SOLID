using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequestValidator : AbstractValidator<GetByIbgeRequest>
    {
        public GetByIbgeRequestValidator()
        {
            RuleFor(req => req.Ibge)
                .NotEmpty()
                .MaximumLength(8);
        }
    }
}
