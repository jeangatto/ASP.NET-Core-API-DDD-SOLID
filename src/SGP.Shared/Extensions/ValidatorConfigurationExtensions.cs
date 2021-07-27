namespace SGP.Shared.Extensions
{
    using Ardalis.GuardClauses;
    using FluentValidation;

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