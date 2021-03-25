using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using SGP.Shared.Notifications;
using System.Text;

namespace SGP.Shared.Extensions
{
    public static class NotifiableExtensions
    {
        /// <summary>
        /// Adiciona as notificações no <see cref="ILogger"/>
        /// </summary>
        /// <param name="notifiable"></param>
        /// <param name="logger"></param>
        /// <param name="logLevel"></param>
        public static void ToLog(this Notifiable notifiable, ILogger logger, LogLevel logLevel = LogLevel.Warning)
        {
            Guard.Against.Null(notifiable, nameof(notifiable));
            Guard.Against.Null(logger, nameof(logger));

            if (!notifiable.IsValid)
            {
                var builder = new StringBuilder();

                foreach (var item in notifiable.Notifications)
                {
                    builder
                        .Append(item.Key)
                        .Append(": ")
                        .AppendLine(item.Message);
                }

                logger.Log(logLevel, builder.ToString());
            }
        }
    }
}
