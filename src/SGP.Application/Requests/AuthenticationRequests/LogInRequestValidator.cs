using FluentValidation;
using SGP.Shared.Extensions;

namespace SGP.Application.Requests.AuthenticationRequests;

public class LogInRequestValidator : AbstractValidator<LogInRequest>
{
    public LogInRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .IsValidEmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(4);
    }
}