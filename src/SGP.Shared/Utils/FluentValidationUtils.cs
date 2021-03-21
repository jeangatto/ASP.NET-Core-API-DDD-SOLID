using FluentValidation;
using System;
using System.Reflection;

namespace SGP.Shared.Utils
{
    /// <summary>
    /// Utilitários para a biblioteca <see cref="FluentValidation"/>.
    /// </summary>
    public static class FluentValidationUtils
    {
        private static readonly Type AbstractValidatorType = typeof(AbstractValidator<>);

        /// <summary>
        /// Cria a instância de um validator (<see cref="IValidator{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>A instância do validator se o mesmo existir ou nulo.</returns>
        public static IValidator<T> GetValidatorInstance<T>() where T : class
        {
            var entityType = typeof(T);
            var genericType = AbstractValidatorType.MakeGenericType(entityType);
            var validatorType = FindValidatorType(entityType.Assembly, genericType);
            return Activator.CreateInstance(validatorType) as IValidator<T>;
        }

        private static Type FindValidatorType(Assembly entityAssembly, Type genericType)
        {
            return Array.Find(entityAssembly.GetTypes(), t => t.IsClass && t.IsSubclassOf(genericType));
        }
    }
}
