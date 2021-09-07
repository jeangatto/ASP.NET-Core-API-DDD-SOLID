using FluentValidation;

namespace SGP.Shared.AppSettings.OptionsValidators
{
    public class AuthOptionsValidator : AbstractValidator<AuthConfig>
    {
        private const short Zero = 0;

        public AuthOptionsValidator()
        {
            RuleFor(options => options.MaximumAttempts)
                .GreaterThan(Zero);

            RuleFor(options => options.SecondsBlocked)
                .GreaterThan(Zero);
        }
    }
}