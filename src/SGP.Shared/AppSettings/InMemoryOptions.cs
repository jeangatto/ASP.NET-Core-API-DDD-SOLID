using SGP.Shared.Abstractions;

namespace SGP.Shared.AppSettings;

public sealed class InMemoryOptions : IAppOptions
{
    public bool Database { get; private init; }
    public bool Cache { get; private init; }
}