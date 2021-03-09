using System;

namespace SGP.Shared.Messages
{
    /// <summary>
    /// Classe base usada pelas respostas da API.
    /// </summary>
    public abstract class BaseResponse : BaseMessage
    {
        protected BaseResponse(Guid correlationId) : base(correlationId)
        {
        }

        protected BaseResponse()
        {
        }
    }
}