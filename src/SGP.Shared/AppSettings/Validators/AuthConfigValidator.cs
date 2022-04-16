using FluentValidation;

namespace SGP.Shared.AppSettings.Validators;

public class AuthConfigValidator : AbstractValidator<AuthConfig>
{
    public AuthConfigValidator()
    {
        RuleFor(options => options.MaximumAttempts).NotEmpty();
        RuleFor(options => options.SecondsBlocked).NotEmpty();
    }
}