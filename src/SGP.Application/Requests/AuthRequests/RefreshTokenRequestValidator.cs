using FluentValidation;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(m => m.Token)
                .NotEmpty();
        }
    }
}