using SGP.Shared.Extensions;
using SGP.Shared.Messages;
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

        public override void Validate()
        {
            new GetByIdRequestValidator().Validate(this).AddToNotifiable(this);
        }
    }
}
