using FluentValidation;

namespace SGP.Shared.AppSettings.OptionsValidators;

public class ConnectionStringsValidator : AbstractValidator<ConnectionStrings>
{
    public ConnectionStringsValidator()
    {
        RuleFor(options => options.DefaultConnection)
            .NotEmpty();
    }
}