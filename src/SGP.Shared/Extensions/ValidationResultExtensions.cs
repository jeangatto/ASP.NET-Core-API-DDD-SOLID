using FluentValidation.Results;
using SGP.Shared.Notifications;

namespace SGP.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Adiciona nas notificações a coleções de erros de uma validação.
        /// </summary>
        /// <param name="validationResult">O resultado de uma validação.</param>
        /// <param name="notifiable"></param>
        public static void AddToNotifiable(this ValidationResult validationResult, Notifiable notifiable)
        {
            if (notifiable != null && validationResult?.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    notifiable.AddNotification(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }
    }
}
