using SGP.Shared.Extensions;
using SGP.Shared.Notifications;
using SGP.Shared.Utils;

namespace SGP.Shared.Messages
{
    /// <summary>
    /// Classe base usada pelas solicitações de API.
    /// </summary>
    public abstract class BaseRequest : Notifiable
    {
        /// <summary>
        /// Valida a requisição.
        /// </summary>
        public abstract void Validate();

        /// <summary>
        /// Valida a entidade e se existirem erros os mesmos serão adicionandos na notificação.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        protected static void ValidateAndAddToNotifiable<T>(T entity) where T : Notifiable
        {
            var validator = FluentValidationUtils.GetValidatorInstance<T>();
            if (validator != null)
            {
                validator.Validate(entity).AddToNotifiable(entity);
            }
        }
    }
}