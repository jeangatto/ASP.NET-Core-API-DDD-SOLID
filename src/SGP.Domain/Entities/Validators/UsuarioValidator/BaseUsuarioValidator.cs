using FluentValidation;

namespace SGP.Domain.Entities.Validators
{
    public abstract class BaseUsuarioValidator : AbstractValidator<Usuario>
    {
        protected void RuleForNome()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(30);
        }

        protected void RuleForEmail()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);
        }

        protected void RuleForSenha()
        {
            RuleFor(x => x.Senha)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(60);
        }
    }
}
