namespace SGP.Shared.Results
{
    public partial class Result : IResult
    {
        internal Result(bool isFailed)
        {
            IsFailed = isFailed;
        }

        internal Result(bool isFailed, string error)
        {
            IsFailed = isFailed;
            Error = error;
        }

        public bool IsFailed { get; }
        public string Error { get; }
        public bool IsSuccess => !IsFailed;
    }
}
