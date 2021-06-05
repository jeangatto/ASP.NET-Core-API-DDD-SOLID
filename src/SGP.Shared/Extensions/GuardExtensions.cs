using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SGP.Shared.Extensions
{
    public static class GuardExtensions
    {
        [SuppressMessage("Redundancy", "RCS1175:Unused this parameter.")]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
        public static void NullOptions<T>(this IGuardClause guardClause, IOptions<T> input, string paramName) where T : class
        {
            if (input == null || input.Value == null)
            {
                throw new ArgumentNullException(paramName,
                    $"A seção '{paramName}' não está configurada no appsettings.json");
            }
        }
    }
}