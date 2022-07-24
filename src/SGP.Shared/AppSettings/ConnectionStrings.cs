using System.ComponentModel.DataAnnotations;

namespace SGP.Shared.AppSettings;

public sealed class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; private set; }
    public string Collation { get; private set; }
}