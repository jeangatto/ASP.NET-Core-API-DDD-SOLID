using System;
using SGP.Shared.Messages;

namespace SGP.Application.Requests
{
    public class GetByIdRequest : BaseRequest
    {
        public GetByIdRequest(Guid id)
        {
            Id = id;
        }

        public GetByIdRequest(string id)
        {
            Id = Guid.TryParse(id, out Guid result) ? result : Guid.Empty;
        }

        public Guid Id { get; private set; }

        public override void Validate()
        {
            ValidationResult = new GetByIdRequestValidator().Validate(this);
        }
    }
}