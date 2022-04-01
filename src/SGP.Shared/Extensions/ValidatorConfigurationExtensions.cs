using FluentValidation;

namespace SGP.Shared.Extensions;

public static class ValidatorConfigurationExtensions
{
    public static void Configure(this ValidatorConfiguration configuration)
    {
        configuration.CascadeMode = CascadeMode.Continue;
        configuration.DisplayNameResolver = (_, member, _) => member?.Name;
    }
}