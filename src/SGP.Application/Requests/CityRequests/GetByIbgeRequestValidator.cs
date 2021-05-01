using FluentValidation;

namespace SGP.Application.Requests.CityRequests
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
