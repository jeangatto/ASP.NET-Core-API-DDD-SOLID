using FluentValidation;

namespace SGP.Shared.AppSettings.OptionsValidators;

public class JwtConfigValidator : AbstractValidator<JwtConfig>
{
    private const short Zero = 0;

    public JwtConfigValidator()
    {
        RuleFor(options => options.Secret)
            .NotEmpty()
            .MinimumLength(32);

        RuleFor(options => options.Seconds)
            .GreaterThan(Zero);

        RuleFor(options => options.Audience)
            .NotEmpty()
            .When(options => options.ValidateAudience);

        RuleFor(options => options.Issuer)
            .NotEmpty()
            .When(options => options.ValidateIssuer);
    }
}