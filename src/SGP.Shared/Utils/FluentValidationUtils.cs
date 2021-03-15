using FluentValidation;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace SGP.Shared.Utils
{
    public static class FluentValidationUtils
    {
        private static readonly ConcurrentDictionary<string, IValidator> _cacheValidatorTypes = new(StringComparer.OrdinalIgnoreCase);
        private static readonly Type AbstractValidatorType = typeof(AbstractValidator<>);

        /// <summary>
        /// Cria a instância de um validator (<see cref="IValidator{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A instância do validator se o mesmo existir ou nulo.</returns>
        public static IValidator<T> GetValidatorInstance<T>() where T : class
        {
            var entityType = typeof(T);
            var cacheKey = entityType.FullName;

            if (_cacheValidatorTypes.ContainsKey(cacheKey))
            {
                return _cacheValidatorTypes[cacheKey] as IValidator<T>;
            }

            var genericType = AbstractValidatorType.MakeGenericType(entityType);
            var validatorType = FindValidatorType(entityType.Assembly, genericType);

            var validatorInstance = Activator.CreateInstance(validatorType) as IValidator<T>;
            if (validatorInstance != null)
            {
                _cacheValidatorTypes.TryAdd(cacheKey, validatorInstance);
            }

            return validatorInstance;
        }

        private static Type FindValidatorType(Assembly entityAssembly, Type genericType)
        {
            return entityAssembly
                .GetTypes()
                .ToList()
                .Find(t => t.IsClass && t.IsSubclassOf(genericType));
        }
    }
}
