namespace SGP.Domain.Validators.UsuarioValidators
{
    public class AlterarEmailValidator : BaseUsuarioValidator
    {
        public AlterarEmailValidator()
        {
            ValidateEmail();
        }
    }
}
