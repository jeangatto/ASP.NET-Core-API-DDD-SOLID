using SGP.Shared.Notifications;
using System.Collections.Generic;

namespace SGP.Shared.Results
{
    public partial class Result
    {
        public static Result Failure(string error) => new(true, error);

        public static Result Failure(IEnumerable<Notification> errors) => new(true, errors);

        public static Result<T> Failure<T>(string error) => new(true, error);

        public static Result<T> Failure<T>(IEnumerable<Notification> errors) => new(true, errors);
    }
}