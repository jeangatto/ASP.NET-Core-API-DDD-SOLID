using FluentValidation;
using SGP.Shared.Extensions;

namespace SGP.Application.Requests.UserRequests
{
    public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .NotEmpty()
                .IsValidEmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(16);
        }
    }
}
