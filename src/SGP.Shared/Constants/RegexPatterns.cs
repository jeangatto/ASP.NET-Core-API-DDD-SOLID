using System.Text.RegularExpressions;

namespace SGP.Shared.Constants
{
    public static class RegexPatterns
    {
        public static readonly Regex ValidEmailAddress = new(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
