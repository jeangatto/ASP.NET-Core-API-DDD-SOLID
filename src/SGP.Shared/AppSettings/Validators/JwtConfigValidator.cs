using FluentValidation;

namespace SGP.Shared.AppSettings.Validators;

public class JwtConfigValidator : AbstractValidator<JwtConfig>
{
    public JwtConfigValidator()
    {
        RuleFor(options => options.Secret).NotEmpty().MinimumLength(32);
        RuleFor(options => options.Seconds).NotEmpty();
    }
}