namespace SGP.Domain.Entities.Validators.UsuarioValidator
{
    public class AlterarEmailValidator : BaseUsuarioValidator
    {
        public AlterarEmailValidator()
        {
            RuleForEmail();
        }
    }
}
