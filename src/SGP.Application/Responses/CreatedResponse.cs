using SGP.Application.Responses.Common;
using System;

namespace SGP.Application.Responses
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
