using System;

namespace SGP.Shared.Messages
{
    /// <summary>
    /// Classe base usada pelas solicitações de API.
    /// </summary>
    public abstract class BaseMessage
    {
        protected BaseMessage(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        protected BaseMessage()
        {
            CorrelationId = Guid.NewGuid();
        }

        /// <summary>
        /// Identificação única usado pelo o logging.
        /// </summary>
        public Guid CorrelationId { get; }
    }
}
