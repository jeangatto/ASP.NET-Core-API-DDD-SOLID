using FluentValidation;

namespace SGP.Application.Requests.UsuarioRequests
{
    public class AddUsuarioRequestValidator : AbstractValidator<AddUsuarioRequest>
    {
        public AddUsuarioRequestValidator()
        {
            RuleFor(req => req.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(req => req.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(req => req.Senha)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(16);
        }
    }
}
