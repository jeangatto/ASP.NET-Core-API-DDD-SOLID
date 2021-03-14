using FluentValidation;
using SGP.Domain.Entities;

namespace SGP.Domain.Validators.UsuarioRequires
{
    public abstract class BaseUsuarioValidator : AbstractValidator<Usuario>
    {
        protected void ValidateNome()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);
        }

        protected void ValidateEmail()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .ChildRules
                (
                    child => child.RuleFor(x => x.Address)
                        .NotEmpty()
                        .EmailAddress()
                        .MaximumLength(100)
                );
        }

        protected void ValidateSenha()
        {
            RuleFor(x => x.Senha)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(60);
        }
    }
}
