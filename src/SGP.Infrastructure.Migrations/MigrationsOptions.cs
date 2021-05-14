using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SGP.Infrastructure.Migrations
{
    internal static class MigrationsOptions
    {
        public static readonly string AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly ILoggerFactory LoggerDbFactory = LoggerFactory.Create(logging => logging.AddConsole());
    }
}