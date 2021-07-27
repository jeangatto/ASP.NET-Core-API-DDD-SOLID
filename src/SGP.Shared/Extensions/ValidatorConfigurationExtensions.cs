using Ardalis.GuardClauses;
using FluentValidation;

namespace SGP.Shared.Extensions
{
    public static class ValidatorConfigurationExtensions
    {
        public static ValidatorConfiguration Configure(this ValidatorConfiguration configuration)
        {
            Guard.Against.Null(configuration, nameof(configuration));
            configuration.DisplayNameResolver = (_, member, _) => member?.Name;
            return configuration;
        }
    }
}