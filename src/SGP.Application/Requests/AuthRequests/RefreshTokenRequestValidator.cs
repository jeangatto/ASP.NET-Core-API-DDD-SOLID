using FluentValidation;

namespace SGP.Application.Requests.AuthRequests
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
        {
            RuleFor(request => request.Token)
                .NotEmpty();
        }
    }
}