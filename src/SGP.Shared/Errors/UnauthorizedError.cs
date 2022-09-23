using System.Diagnostics.CodeAnalysis;
using FluentResults;

namespace SGP.Shared.Errors;

[ExcludeFromCodeCoverage]
public sealed class UnauthorizedError : Error
{
    public UnauthorizedError()
    {
    }

    public UnauthorizedError(string message) : base(message)
    {
    }
}