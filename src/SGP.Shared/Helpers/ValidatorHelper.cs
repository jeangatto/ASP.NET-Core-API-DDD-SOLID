using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace SGP.Shared.Helpers;

public static class ValidatorHelper
{
    private static readonly ConcurrentDictionary<string, Lazy<IValidator>> Cache = new();

    public static ValidationResult Validate<TValidator>(object instanceToValidate)
        where TValidator : IValidator
    {
        var context = new ValidationContext<object>(instanceToValidate);
        var validator = CreateOrGetValidatorInstance<TValidator>();
        return validator.Value.Validate(context);
    }

    public static async Task<ValidationResult> ValidateAsync<TValidator>(object instanceToValidate)
        where TValidator : IValidator
    {
        var context = new ValidationContext<object>(instanceToValidate);
        var validator = CreateOrGetValidatorInstance<TValidator>();
        return await validator.Value.ValidateAsync(context);
    }

    private static Lazy<IValidator> CreateOrGetValidatorInstance<TValidator>() where TValidator : IValidator
    {
        var lazyValidator = new Lazy<IValidator>(() =>
            Activator.CreateInstance<TValidator>(), LazyThreadSafetyMode.ExecutionAndPublication);

        return Cache.GetOrAdd(typeof(TValidator).Name, _ => lazyValidator);
    }
}