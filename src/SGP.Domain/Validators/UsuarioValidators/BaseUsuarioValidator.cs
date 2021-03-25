using FluentValidation;
using SGP.Domain.Entities;

namespace SGP.Domain.Validators.UsuarioValidators
{
    public abstract class BaseUsuarioValidator : AbstractValidator<Usuario>
    {
        protected void ValidateNome()
        {
            RuleFor(u => u.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);
        }

        protected void ValidateEmail()
        {
            RuleFor(u => u.Email)
                .NotNull()
                .ChildRules
                (
                    child => child.RuleFor(e => e.Address)
                        .NotEmpty()
                        .EmailAddress()
                        .MaximumLength(100)
                );
        }

        protected void ValidateSenha()
        {
            RuleFor(u => u.Senha)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(60);
        }
    }
}
