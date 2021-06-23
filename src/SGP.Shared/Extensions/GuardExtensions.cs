using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SGP.Shared.Extensions
{
    public static class GuardExtensions
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="input"/> is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="paramName"></param>
        /// <returns><paramref name="input"/> if the value is not null.</returns>
        [SuppressMessage("Redundancy", "RCS1175:Unused this parameter.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        public static IOptions<T> NullOptions<T>(this IGuardClause guardClause, IOptions<T> input,
            string paramName) where T : class
        {
            if (input == null || input.Value == null)
            {
                throw new ArgumentNullException(paramName,
                    $"A seção '{typeof(T).Name}' não está configurada no appsettings.json");
            }

            return input;
        }
    }
}