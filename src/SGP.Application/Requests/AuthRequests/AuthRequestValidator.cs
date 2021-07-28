using FluentValidation;
using SGP.Shared.Extensions;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(m => m.Email)
                .NotEmpty()
                .IsValidEmailAddress()
                .MaximumLength(100);

            RuleFor(m => m.Password)
                .NotEmpty();
        }
    }
}