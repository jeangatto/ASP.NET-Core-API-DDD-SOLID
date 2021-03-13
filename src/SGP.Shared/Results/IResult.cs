using SGP.Shared.Notifications;
using System.Collections.Generic;

namespace SGP.Shared.Results
{
    public interface IResult
    {
        string Message { get; }
        bool Succeeded { get; }
        IEnumerable<Notification> Notifications { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}