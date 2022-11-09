using System.ComponentModel.DataAnnotations;

namespace SGP.Shared.AppSettings;

public sealed class ConnectionOptions
{
    public const string ConfigSectionPath = "ConnectionStrings";

    [Required]
    public string DefaultConnection { get; private init; }
    public string Collation { get; private init; }
}