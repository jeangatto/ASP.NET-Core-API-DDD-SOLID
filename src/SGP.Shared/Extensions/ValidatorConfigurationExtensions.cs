using FluentValidation;

namespace SGP.Shared.Extensions;

public static class ValidatorConfigurationExtensions
{
    public static void Configure(this ValidatorConfiguration configuration)
        => configuration.DisplayNameResolver = (_, member, _) => member?.Name;
}