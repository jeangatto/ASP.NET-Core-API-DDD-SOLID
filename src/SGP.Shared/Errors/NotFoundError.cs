using FluentResults;

namespace SGP.Shared.Errors;

public sealed class NotFoundError : Error
{
    public NotFoundError(string message) : base(message)
    {
    }

    public NotFoundError(string message, Error causedBy) : base(message, causedBy)
    {
    }
}