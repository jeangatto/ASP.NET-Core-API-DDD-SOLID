using FluentValidation;

namespace SGP.Application.Requests.CidadeRequests
{
    public class GetByIbgeRequestValidator : AbstractValidator<GetByIbgeRequest>
    {
        public GetByIbgeRequestValidator()
        {
            RuleFor(request => request.Ibge)
                .NotEmpty()
                .MaximumLength(8);
        }
    }
}
