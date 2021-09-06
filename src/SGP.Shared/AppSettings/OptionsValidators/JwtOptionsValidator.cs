using FluentValidation;

namespace SGP.Shared.AppSettings.OptionsValidators
{
    public class JwtOptionsValidator : AbstractValidator<JwtConfig>
    {
        public JwtOptionsValidator()
        {
            RuleFor(jwt => jwt.Secret)
                .NotEmpty()
                .MinimumLength(32);

            RuleFor(jwt => jwt.Seconds)
                .GreaterThan((short)0);

            RuleFor(jwt => jwt.Audience)
                .NotEmpty()
                .When(jwt => jwt.ValidateAudience);

            RuleFor(jwt => jwt.Issuer)
                .NotEmpty()
                .When(jwt => jwt.ValidateIssuer);
        }
    }
}