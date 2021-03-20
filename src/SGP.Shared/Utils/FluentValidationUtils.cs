using FluentValidation;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace SGP.Shared.Utils
{
    /// <summary>
    /// Utilitários para a biblioteca <see cref="FluentValidation"/>.
    /// </summary>
    public static class FluentValidationUtils
    {
        private static readonly ConcurrentDictionary<string, IValidator> _cachedValidatorInstances = new(StringComparer.OrdinalIgnoreCase);
        private static readonly Type AbstractValidatorType = typeof(AbstractValidator<>);

        /// <summary>
        /// Cria a instância de um validator (<see cref="IValidator{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheValidator">Adiciona no cache (memória) a instância do validador.</param>
        /// <returns>A instância do validator se o mesmo existir ou nulo.</returns>
        public static IValidator<T> GetValidatorInstance<T>(bool cacheValidator = false) where T : class
        {
            var entityType = typeof(T);
            var cacheKey = entityType.FullName;

            if (_cachedValidatorInstances.ContainsKey(cacheKey))
            {
                return _cachedValidatorInstances[cacheKey] as IValidator<T>;
            }

            var genericType = AbstractValidatorType.MakeGenericType(entityType);
            var validatorType = FindValidatorType(entityType.Assembly, genericType);

            var validatorInstance = Activator.CreateInstance(validatorType) as IValidator<T>;
            if (cacheValidator && validatorInstance != null)
            {
                _cachedValidatorInstances.TryAdd(cacheKey, validatorInstance);
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
