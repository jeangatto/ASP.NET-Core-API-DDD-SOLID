using FluentValidation;

namespace SGP.Shared.AppSettings.Validators;

public class CacheConfigValidator : AbstractValidator<CacheConfig>
{
    public CacheConfigValidator()
    {
        RuleFor(options => options.AbsoluteExpirationInHours).NotEmpty();
        RuleFor(options => options.SlidingExpirationInSeconds).NotEmpty();
    }
}