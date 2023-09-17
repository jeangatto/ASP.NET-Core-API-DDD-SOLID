using Bogus;

namespace SGP.Tests.Extensions;

internal static class RandomizerExtensions
{
    public static string JsonWebToken(this Randomizer randomizer)
        => randomizer.String2(2048, "_/-abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
}
