using System;
using System.ComponentModel.DataAnnotations;

namespace SGP.Shared.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class RequiredGreaterThanZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
        => value != null && int.TryParse(value.ToString(), out var result) && result > 0;
}