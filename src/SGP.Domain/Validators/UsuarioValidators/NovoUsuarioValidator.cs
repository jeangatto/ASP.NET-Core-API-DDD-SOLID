namespace SGP.Domain.Validators.UsuarioValidators
{
    public class NovoUsuarioValidator : BaseUsuarioValidator
    {
        public NovoUsuarioValidator()
        {
            ValidateNome();
            ValidateEmail();
            ValidateSenha();
        }
    }
}