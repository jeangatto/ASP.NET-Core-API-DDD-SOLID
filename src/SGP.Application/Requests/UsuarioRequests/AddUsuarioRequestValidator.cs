using FluentValidation;

namespace SGP.Application.Requests.UsuarioRequests
{
    public class AddUsuarioRequestValidator : AbstractValidator<AddUsuarioRequest>
    {
        public AddUsuarioRequestValidator()
        {
            RuleFor(request => request.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);

            RuleFor(request => request.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(request => request.Senha)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(16);
        }
    }
}
