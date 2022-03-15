using FluentValidation;

namespace SGP.Application.Requests.AuthenticationRequests
{
    public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshTokenRequestValidator()
            => RuleFor(x => x.Token).NotEmpty();
    }
}