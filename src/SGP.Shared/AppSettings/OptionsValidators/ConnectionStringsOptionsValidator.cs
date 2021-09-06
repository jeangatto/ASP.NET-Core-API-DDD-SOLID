using FluentValidation;

namespace SGP.Shared.AppSettings.OptionsValidators
{
    public class ConnectionStringsOptionsValidator : AbstractValidator<ConnectionStrings>
    {
        public ConnectionStringsOptionsValidator()
        {
            RuleFor(options => options.DefaultConnection)
                .NotEmpty();
        }
    }
}