using Ardalis.GuardClauses;
using FluentValidation.Results;
using SGP.Shared.Notifications;

namespace SGP.Shared.Extensions
{
    public static class ValidationResultExtensions
    {
        public static void AddToNotifiable(this ValidationResult validationResult, Notifiable notifiable)
        {
            Guard.Against.Null(notifiable, nameof(notifiable));

            if (validationResult?.IsValid == false)
            {
                foreach (var failure in validationResult.Errors)
                {
                    notifiable.AddNotification(failure.PropertyName, failure.ErrorMessage);
                }
            }
        }
    }
}