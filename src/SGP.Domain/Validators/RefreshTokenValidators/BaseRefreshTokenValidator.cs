using FluentValidation;
using SGP.Domain.Entities;

namespace SGP.Domain.Validators.RefreshTokenValidators
{
    public abstract class BaseRefreshTokenValidator : AbstractValidator<RefreshToken>
    {
        protected void ValidateToken()
        {
            RuleFor(t => t.Token)
                .NotEmpty()
                .MaximumLength(RefreshToken.MAX_TOKEN_SIZE);
        }

        protected void ValidateRevokedAt()
        {
            RuleFor(t => t.RevokedAt)
                .NotNull()
                .GreaterThan(t => t.CreatedAt);
        }

        protected void ValidateReplacedByToken()
        {
            RuleFor(t => t.ReplacedByToken)
                .NotEmpty()
                .MaximumLength(RefreshToken.MAX_TOKEN_SIZE);
        }
    }
}
