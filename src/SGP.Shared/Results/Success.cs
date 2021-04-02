namespace SGP.Shared.Results
{
    public partial class Result
    {
        public static Result Success() => new(true);

        public static Result<T> Success<T>(T value) => new(true, value);
    }
}
