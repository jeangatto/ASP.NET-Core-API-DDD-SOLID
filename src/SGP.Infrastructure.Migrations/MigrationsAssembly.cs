using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SGP.Infrastructure.Migrations
{
    internal static class MigrationsAssembly
    {
        public static readonly string Name = Assembly.GetExecutingAssembly().GetName().Name;
        public static readonly ILoggerFactory LoggerDbFactory = LoggerFactory.Create(logging => logging.AddConsole());
    }
}