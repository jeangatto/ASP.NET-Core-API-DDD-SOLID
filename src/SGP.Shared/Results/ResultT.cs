namespace SGP.Shared.Results
{
    public class Result<T> : Result, IResult<T>
    {
        internal Result(bool isFailed) : base(isFailed)
        {
        }

        internal Result(bool isFailed, T value) : base(isFailed)
        {
            Value = value;
        }

        internal Result(bool isFailed, string error) : base(isFailed, error)
        {
        }

        public T Value { get; }
    }
}
