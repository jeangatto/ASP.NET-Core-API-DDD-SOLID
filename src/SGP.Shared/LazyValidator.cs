using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace SGP.Shared;

public static class LazyValidator
{
    private static readonly ConcurrentDictionary<string, Lazy<IValidator>> ValidadorLookUp = new(StringComparer.OrdinalIgnoreCase);

    public static ValidationResult Validate<TValidator, TInstance>(TInstance instanceToValidate)
        where TValidator : IValidator
    {
        var context = new ValidationContext<TInstance>(instanceToValidate);
        var lazyValidator = CreateOrGetValidatorInstance<TValidator>();
        return lazyValidator.Value.Validate(context);
    }

    public static async Task<ValidationResult> ValidateAsync<TValidator, TInstance>(TInstance instanceToValidate)
        where TValidator : IValidator
    {
        var context = new ValidationContext<TInstance>(instanceToValidate);
        var lazyValidator = CreateOrGetValidatorInstance<TValidator>();
        return await lazyValidator.Value.ValidateAsync(context);
    }

    private static Lazy<IValidator> CreateOrGetValidatorInstance<TValidator>() where TValidator : IValidator
    {
        var lazyValidator = new Lazy<IValidator>(() =>
            Activator.CreateInstance<TValidator>(), true);

        return ValidadorLookUp.GetOrAdd(typeof(TValidator).Name, lazyValidator);
    }
}