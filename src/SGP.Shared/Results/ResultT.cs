using SGP.Shared.Notifications;
using System.Collections.Generic;

namespace SGP.Shared.Results
{
    public class Result<T> : Result, IResult<T>
    {
        internal Result(bool isSuccess, T value)
            : base(isSuccess)
        {
            Value = value;
        }

        internal Result(bool isSuccess, string error)
            : base(isSuccess, error)
        {
        }

        internal Result(bool isSuccess, IEnumerable<Notification> errors)
            : base(isSuccess, errors)
        {
        }

        public T Value { get; }
    }
}
