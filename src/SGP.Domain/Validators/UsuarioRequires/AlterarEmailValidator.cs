namespace SGP.Domain.Validators.UsuarioRequires
{
    public class AlterarEmailValidator : BaseUsuarioValidator
    {
        public AlterarEmailValidator()
        {
            ValidateEmail();
        }
    }
}
