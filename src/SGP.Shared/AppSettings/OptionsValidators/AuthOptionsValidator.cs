using FluentValidation;

namespace SGP.Shared.AppSettings.OptionsValidators
{
    public class AuthOptionsValidator : AbstractValidator<AuthConfig>
    {
        public AuthOptionsValidator()
        {
            RuleFor(options => options.MaximumAttempts)
                .GreaterThan((short)0);

            RuleFor(options => options.SecondsBlocked)
                .GreaterThan((short)0);
        }
    }
}