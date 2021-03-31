using FluentValidation;

namespace SGP.Application.Requests.AuthRequests
{
    public class AuthRequestValidator : AbstractValidator<AuthRequest>
    {
        public AuthRequestValidator()
        {
            RuleFor(request => request.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(request => request.Senha)
                .NotEmpty();
        }
    }
}
