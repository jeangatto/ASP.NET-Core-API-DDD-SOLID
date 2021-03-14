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
            this.Succeeded = false;
            return this;
        }

        public IResult Fail(string message)
        {
            this.Succeeded = false;
            this.Message = message;
            return this;
        }

        public IResult Fail(string message, IEnumerable<Notification> notifications)
        {
            this.Succeeded = false;
            this.Message = message;
            this.Notifications = notifications;
            return this;
        }

        public IResult Fail(IEnumerable<Notification> notifications)
        {
            this.Succeeded = false;
            this.Notifications = notifications;
            return this;
        }

        public IResult Fail(string message, Notification notification)
        {
            this.Succeeded = false;
            this.Message = message;
            this.Notifications = new[] { notification };
            return this;
        }

        public IResult Fail(Notification notification)
        {
            this.Succeeded = false;
            this.Notifications = new[] { notification };
            return this;
        }

        public IResult Success()
        {
            this.Succeeded = true;
            return this;
        }

        public IResult Success(string message)
        {
            this.Succeeded = true;
            this.Message = message;
            return this;
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; private set; }

        public new IResult<T> Fail()
        {
            this.Succeeded = false;
            return this;
        }

        public new IResult<T> Fail(string message)
        {
            this.Succeeded = false;
            this.Message = message;
            return this;
        }

        public new IResult<T> Fail(string message, IEnumerable<Notification> notifications)
        {
            this.Succeeded = false;
            this.Message = message;
            this.Notifications = notifications;
            return this;
        }

        public new IResult<T> Fail(IEnumerable<Notification> notifications)
        {
            this.Succeeded = false;
            this.Notifications = notifications;
            return this;
        }

        public new IResult<T> Fail(string message, Notification notification)
        {
            this.Succeeded = false;
            this.Message = message;
            this.Notifications = new[] { notification };
            return this;
        }

        public new IResult<T> Fail(Notification notification)
        {
            this.Succeeded = false;
            this.Notifications = new[] { notification };
            return this;
        }

        public new IResult<T> Success()
        {
            this.Succeeded = true;
            return this;
        }

        public new IResult<T> Success(string message)
        {
            this.Succeeded = true;
            this.Message = message;
            return this;
        }

        public IResult<T> Success(T data)
        {
            this.Succeeded = true;
            this.Data = data;
            return this;
        }

        public IResult<T> Success(T data, string message)
        {
            this.Succeeded = true;
            this.Message = message;
            this.Data = data;
            return this;
        }
    }
}
