using SGP.Shared.Notifications;

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
}