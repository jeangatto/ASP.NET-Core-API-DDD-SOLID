namespace SGP.Domain.Validators.RefreshTokenValidators
{
    public class AlterarRefreshTokenValidator : BaseRefreshTokenValidator
    {
        public AlterarRefreshTokenValidator()
        {
            ValidateReplacedByToken();
            ValidateRevokedAt();
        }
    }
}