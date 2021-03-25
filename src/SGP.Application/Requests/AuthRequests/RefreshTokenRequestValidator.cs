using FluentValidation;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}