using System;
using FluentValidation;

namespace SGP.Shared.Entities
{
    public abstract class BaseValidator
    {
        protected static void Validate<TValidator>(object instance) where TValidator : IValidator
        {
            var context = new ValidationContext<object>(instance);
            var validator = Activator.CreateInstance<TValidator>();
            var result = validator.Validate(context);
            if (result.Errors.Count > 0)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}