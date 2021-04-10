namespace SGP.Shared.Results
{
    public interface IResult
    {
        bool IsFailed { get; }
        string Error { get; }
        bool IsSuccess { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}