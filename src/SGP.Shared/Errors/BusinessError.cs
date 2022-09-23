using System.Diagnostics.CodeAnalysis;
using FluentResults;

namespace SGP.Shared.Errors;

[ExcludeFromCodeCoverage]
public sealed class BusinessError : Error
{
    public BusinessError(string message) : base(message)
    {
    }
}