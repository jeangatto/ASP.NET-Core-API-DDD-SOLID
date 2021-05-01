using FluentValidation;

namespace SGP.Application.Requests.CityRequests
{
    public class GetAllByStateAbbrRequestValidator : AbstractValidator<GetAllByStateAbbrRequest>
    {
        public GetAllByStateAbbrRequestValidator()
        {
            RuleFor(x => x.StateAbbr)
                .NotEmpty()
                .Length(2);
        }
    }
}
