namespace SGP.Shared.Results
{
    public interface IResult
    {
        string Message { get; }
        bool Succeeded { get; }
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}