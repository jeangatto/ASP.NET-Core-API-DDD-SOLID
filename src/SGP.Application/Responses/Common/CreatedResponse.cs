using SGP.Shared.Messages;
using System;

namespace SGP.Application.Responses.Common
{
    public sealed class CreatedResponse : BaseResponse
    {
        public CreatedResponse(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
