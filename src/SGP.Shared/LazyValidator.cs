using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace SGP.Shared;

public static class LazyValidator
{
    private static readonly ConcurrentDictionary<string, Lazy<IValidator>> Cache = new();

    public static ValidationResult Validate<TValidator>(object instanceToValidate)
        where TValidator : IValidator
    {
        var context = new ValidationContext<object>(instanceToValidate);
        var lazyValidator = CreateOrGetValidatorInstance<TValidator>();
        return lazyValidator.Value.Validate(context);
    }

    public static async Task<ValidationResult> ValidateAsync<TValidator>(object instanceToValidate)
        where TValidator : IValidator
    {
        var context = new ValidationContext<object>(instanceToValidate);
        var lazyValidator = CreateOrGetValidatorInstance<TValidator>();
        return await lazyValidator.Value.ValidateAsync(context);
    }

    private static Lazy<IValidator> CreateOrGetValidatorInstance<TValidator>() where TValidator : IValidator
    {
        var lazyValidator = new Lazy<IValidator>(() =>
            Activator.CreateInstance<TValidator>(), isThreadSafe: true);

        return Cache.GetOrAdd(typeof(TValidator).Name, _ => lazyValidator);
    }
}
