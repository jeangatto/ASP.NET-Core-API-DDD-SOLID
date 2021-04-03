namespace SGP.Shared.Results
{
    public partial class Result : IResult
    {
        internal Result(bool failed)
        {
            Failed = failed;
        }

        internal Result(bool failed, string error)
        {
            Failed = failed;
            Error = error;
        }

        public bool Failed { get; }
        public string Error { get; }
        public bool Succeeded => !Failed;
    }
}
