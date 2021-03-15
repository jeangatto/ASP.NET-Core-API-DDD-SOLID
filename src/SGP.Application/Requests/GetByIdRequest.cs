using SGP.Shared.Messages;
using System;

namespace SGP.Application.Requests
{
    public sealed class GetByIdRequest : BaseRequest<GetByIdRequest>
    {
        public GetByIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
