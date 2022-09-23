using System.Diagnostics.CodeAnalysis;
using FluentResults;

namespace SGP.Shared.Errors;

[ExcludeFromCodeCoverage]
public sealed class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
    }
}