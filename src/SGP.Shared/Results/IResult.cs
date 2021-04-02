using SGP.Shared.Notifications;
using System.Collections.Generic;

namespace SGP.Shared.Results
{
    public interface IResult
    {
        bool IsSuccess { get; }
        IEnumerable<Notification> Errors { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}