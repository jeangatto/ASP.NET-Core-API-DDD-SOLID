using System;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;

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
        public static void NullOptions<T>(this IGuardClause guardClause, IOptions<T> input, string paramName)
            where T : class
        {
            if (input?.Value == null)
            {
                throw new ArgumentNullException(paramName,
                    $"A seção '{typeof(T).Name}' não está configurada no appsettings.json");
            }
        }
    }
}