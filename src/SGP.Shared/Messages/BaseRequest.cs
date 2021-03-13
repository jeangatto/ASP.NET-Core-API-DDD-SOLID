using SGP.Shared.Notifications;
using System;

namespace SGP.Shared.Messages
{
    /// <summary>
    /// Classe base usada pelas solicitações de API.
    /// </summary>
    public abstract class BaseRequest : Notifiable
    {
        protected BaseRequest(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        protected BaseRequest()
        {
            CorrelationId = Guid.NewGuid();
        }

        /// <summary>
        /// Identificação única usado pelo o log.
        /// </summary>
        public Guid CorrelationId { get; }

        /// <summary>
        /// Valida a requisição.
        /// </summary>
        public abstract void Validate();
    }
}