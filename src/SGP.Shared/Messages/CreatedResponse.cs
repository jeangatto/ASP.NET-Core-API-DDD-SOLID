using System;

namespace SGP.Shared.Messages
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
