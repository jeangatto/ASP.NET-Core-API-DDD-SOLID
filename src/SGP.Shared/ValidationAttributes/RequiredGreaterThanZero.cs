using System;
using System.ComponentModel.DataAnnotations;

namespace SGP.Shared.ValidationAttributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class RequiredGreaterThanZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
        => value != null && int.TryParse(value.ToString(), out var result) && result > 0;
}