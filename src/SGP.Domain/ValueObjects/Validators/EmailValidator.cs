using FluentValidation;
using SGP.Shared.Extensions;

namespace SGP.Domain.ValueObjects.Validators
{
    public class EmailValidator : AbstractValidator<Email>
    {
        public EmailValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty()
                .IsValidEmailAddress()
                .MaximumLength(100);
        }
    }
}