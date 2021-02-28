namespace SGP.Domain.Validators.UsuarioRequires
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