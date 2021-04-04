using SGP.Application.Requests.Common;
using System;

namespace SGP.Application.Requests
{
    public sealed class GetByIdRequest : BaseRequest
    {
        public GetByIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
