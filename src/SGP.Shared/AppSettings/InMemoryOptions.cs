using SGP.Shared.Abstractions;

namespace SGP.Shared.AppSettings;

public sealed class InMemoryOptions : IAppOptions
{
    public static string ConfigSectionPath => "InMemoryOptions";

    public bool Database { get; private init; }
    public bool Cache { get; private init; }
}