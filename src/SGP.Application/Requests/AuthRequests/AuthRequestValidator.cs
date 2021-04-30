using FluentValidation;
using SGP.Shared.Extensions;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .IsValidEmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Senha)
                .NotEmpty();
        }
    }
}
