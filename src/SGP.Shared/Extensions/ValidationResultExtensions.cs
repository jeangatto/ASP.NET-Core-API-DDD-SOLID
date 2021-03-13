using Ardalis.GuardClauses;
using FluentValidation;
using FluentValidation.Results;
using SGP.Shared.Notifications;

namespace SGP.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Adiciona nas notificações a coleções de erros de uma validação.
        /// </summary>
        /// <param name="result">O resultado de uma validação.</param>
        /// <param name="notifiable"></param>
        public static void AddToNotifiable(this ValidationResult result, Notifiable notifiable)
        {
            Guard.Against.Null(notifiable, nameof(notifiable));

            if (result?.IsValid == false)
            {
                foreach (var failure in result.Errors)
                {
                    notifiable.AddNotification(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }
    }
}
