using FluentValidation;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Senha)
                .NotEmpty();
        }
    }
}
