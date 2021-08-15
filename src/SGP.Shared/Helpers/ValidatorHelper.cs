﻿using System;
using System.Collections.Concurrent;
using FluentValidation;
using FluentValidation.Results;

namespace SGP.Shared.Helpers
{
    public static class ValidatorHelper
    {
        private static readonly ConcurrentDictionary<string, IValidator> Cache = new();

        public static ValidationResult Validate<TValidator>(object instanceToValidate) where TValidator : IValidator
        {
            var context = new ValidationContext<object>(instanceToValidate);
            return CreateOrGetValidatorInstance<TValidator>().Validate(context);
        }

        private static IValidator CreateOrGetValidatorInstance<TValidator>() where TValidator : IValidator
            => Cache.GetOrAdd(typeof(TValidator).Name, Activator.CreateInstance<TValidator>());
    }
}