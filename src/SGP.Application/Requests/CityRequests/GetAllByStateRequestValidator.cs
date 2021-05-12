using FluentValidation;

namespace SGP.Application.Requests.CityRequests
{
    public class GetAllByStateRequestValidator : AbstractValidator<GetAllByStateRequest>
    {
        public GetAllByStateRequestValidator()
        {
            RuleFor(x => x.StateAbbr)
                .NotEmpty()
                .Length(2);
        }
    }
}
