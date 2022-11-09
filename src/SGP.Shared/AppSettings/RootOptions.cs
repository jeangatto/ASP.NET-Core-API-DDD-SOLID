namespace SGP.Shared.AppSettings;

public sealed class RootOptions
{
    public const string ConfigSectionPath = "";

    public bool InMemoryDatabase { get; private init; }
}