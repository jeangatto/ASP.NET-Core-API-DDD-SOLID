using System;

namespace SGP.Application.Responses.Common
{
    public sealed class CreatedResponse
    {
        public CreatedResponse(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
