using FluentValidation;
using SGP.Shared.Extensions;

namespace SGP.Application.Requests.AuthenticationRequests;

public class LogInRequestValidator : AbstractValidator<LogInRequest>
{
    public LogInRequestValidator()
    {
        RuleFor(req => req.Email)
            .NotEmpty()
            .IsValidEmailAddress()
            .MaximumLength(100);

        RuleFor(req => req.Password)
            .NotEmpty()
            .MinimumLength(4);
    }
}
