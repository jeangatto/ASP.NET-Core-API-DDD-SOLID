using FluentValidation;

namespace SGP.Shared.AppSettings.OptionsValidators
{
    public class AuthConfigValidator : AbstractValidator<AuthConfig>
    {
        private const short Zero = 0;

        public AuthConfigValidator()
        {
            RuleFor(options => options.MaximumAttempts)
                .GreaterThan(Zero);

            RuleFor(options => options.SecondsBlocked)
                .GreaterThan(Zero);
        }
    }
}