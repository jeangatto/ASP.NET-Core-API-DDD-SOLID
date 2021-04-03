using System;

namespace SGP.Application.Requests
{
    public sealed class GetByIdRequest
    {
        public GetByIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
