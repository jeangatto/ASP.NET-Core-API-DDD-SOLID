using SGP.Shared.Messages;
using System;

namespace SGP.Application.Requests
{
    public class GetByIdRequest : BaseRequest
    {
        public GetByIdRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public override void Validate() => ValidateAndAddToNotifiable(this);
    }
}
