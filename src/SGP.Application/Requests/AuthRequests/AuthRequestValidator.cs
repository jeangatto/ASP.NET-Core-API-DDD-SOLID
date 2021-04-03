using FluentValidation;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(req => req.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(req => req.Senha)
                .NotEmpty();
        }
    }
}
