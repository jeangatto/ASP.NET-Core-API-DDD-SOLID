namespace SGP.Shared.Results
{
    public class Result<T> : Result, IResult<T>
    {
        internal Result(bool failed, T value)
            : base(failed)
        {
            Value = value;
        }

        internal Result(bool failed, string error)
            : base(failed, error)
        {
        }

        public T Value { get; }
    }
}
