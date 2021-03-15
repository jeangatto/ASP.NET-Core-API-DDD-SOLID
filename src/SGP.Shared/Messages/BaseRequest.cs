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
    }

    /// <inheritdoc/>
    public abstract class BaseRequest<T> : BaseRequest where T : Notifiable
    {
        public override void Validate()
        {
            var validator = FluentValidationUtils.GetValidatorInstance<T>();
            if (validator != null)
            {
                var entity = this as T;
                validator.Validate(entity).AddToNotifiable(entity);
            }
        }
    }
}