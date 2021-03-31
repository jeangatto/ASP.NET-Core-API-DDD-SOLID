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
        public static void Null<TOptions>(this IGuardClause guardClause, IOptions<TOptions> options)
            where TOptions : class
        {
            if (options == null || options.Value == null)
            {
                throw new ArgumentNullException(nameof(options),
                    $"A seção '{typeof(TOptions).Name}' não está configurada no appsettings.json");
            }
        }
    }
}