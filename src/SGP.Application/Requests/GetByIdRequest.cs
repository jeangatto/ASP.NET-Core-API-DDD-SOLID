using SGP.Shared.Messages;
using System;

namespace SGP.Application.Requests
{
    public class GetByIdRequest : BaseRequest
    {
        public GetByIdRequest(Guid id) => Id = id;

        public GetByIdRequest(string id)
        {
            Id = Guid.TryParse(id, out Guid result) ? result : Guid.Empty;
        }

        public Guid Id { get; }

        public override void Validate()
        {
            ValidationResult = new GetByIdRequestValidator().Validate(this);
        }

        public override string ToString() => Id.ToString();
    }
}
