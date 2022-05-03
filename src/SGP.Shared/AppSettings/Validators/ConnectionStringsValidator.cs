using FluentValidation;

namespace SGP.Shared.AppSettings.Validators;

public class ConnectionStringsValidator : AbstractValidator<ConnectionStrings>
{
    public ConnectionStringsValidator()
        => RuleFor(options => options.DefaultConnection).NotEmpty();
}