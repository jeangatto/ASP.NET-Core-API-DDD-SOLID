using SGP.Shared.Notifications;
using System.Collections.Generic;
using System.Linq;

namespace SGP.Shared.Results
{
    public class Result : IResult
    {
        public Result()
        {
            Notifications = Enumerable.Empty<Notification>();
        }

        public string Message { get; protected set; }
        public bool Succeeded { get; protected set; }
        public IEnumerable<Notification> Notifications { get; protected set; }

        public IResult Fail()
        {
            Succeeded = false;
            return this;
        }

        public IResult Fail(string message)
        {
            Succeeded = false;
            Message = message;
            return this;
        }

        public IResult Fail(string message, IEnumerable<Notification> notifications)
        {
            Succeeded = false;
            Message = message;
            Notifications = notifications;
            return this;
        }

        public IResult Fail(IEnumerable<Notification> notifications)
        {
            Succeeded = false;
            Notifications = notifications;
            return this;
        }

        public IResult Fail(string message, Notification notification)
        {
            Succeeded = false;
            Message = message;
            Notifications = new[] { notification };
            return this;
        }

        public IResult Fail(Notification notification)
        {
            Succeeded = false;
            Notifications = new[] { notification };
            return this;
        }

        public IResult Success()
        {
            Succeeded = true;
            return this;
        }

        public IResult Success(string message)
        {
            Succeeded = true;
            Message = message;
            return this;
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; private set; }

        public new IResult<T> Fail()
        {
            Succeeded = false;
            return this;
        }

        public new IResult<T> Fail(string message)
        {
            Succeeded = false;
            Message = message;
            return this;
        }

        public new IResult<T> Fail(string message, IEnumerable<Notification> notifications)
        {
            Succeeded = false;
            Message = message;
            Notifications = notifications;
            return this;
        }

        public new IResult<T> Fail(IEnumerable<Notification> notifications)
        {
            Succeeded = false;
            Notifications = notifications;
            return this;
        }

        public new IResult<T> Fail(string message, Notification notification)
        {
            Succeeded = false;
            Message = message;
            Notifications = new[] { notification };
            return this;
        }

        public new IResult<T> Fail(Notification notification)
        {
            Succeeded = false;
            Notifications = new[] { notification };
            return this;
        }

        public new IResult<T> Success()
        {
            Succeeded = true;
            return this;
        }

        public new IResult<T> Success(string message)
        {
            Succeeded = true;
            Message = message;
            return this;
        }

        public IResult<T> Success(T data)
        {
            Succeeded = true;
            Data = data;
            return this;
        }

        public IResult<T> Success(T data, string message)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            return this;
        }
    }
}
