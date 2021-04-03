namespace SGP.Shared.Results
{
    public interface IResult
    {
        bool Failed { get; }
        string Error { get; }
        bool Succeeded { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Value { get; }
    }
}