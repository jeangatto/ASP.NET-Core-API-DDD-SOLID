using SGP.Application.Requests.Common;
using System;

namespace SGP.Application.Requests
{
    public sealed class GetByIdRequest : BaseRequest
    {
        public GetByIdRequest(string id)
        {
            if (Guid.TryParse(id, out Guid result))
            {
                Id = result;
            }
            else
            {
                Id = Guid.Empty;
            }
        }

        public GetByIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
