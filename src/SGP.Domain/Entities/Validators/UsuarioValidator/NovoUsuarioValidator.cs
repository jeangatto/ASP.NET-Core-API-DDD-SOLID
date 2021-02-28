namespace SGP.Domain.Entities.Validators
{
    public class NovoUsuarioValidator : BaseUsuarioValidator
    {
        public NovoUsuarioValidator()
        {
            RuleForNome();
            RuleForEmail();
            RuleForSenha();
        }
    }
}