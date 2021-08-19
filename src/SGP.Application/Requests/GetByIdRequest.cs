using System;
using SGP.Shared.Helpers;
using SGP.Shared.Messages;

namespace SGP.Application.Requests
{
    public class GetByIdRequest : BaseRequest
    {
        public GetByIdRequest(Guid id) => Id = id;

        public GetByIdRequest(string id) => Id = Guid.TryParse(id, out var result) ? result : Guid.Empty;

        public Guid Id { get; }

        public override void Validate()
        {
            ValidationResult = ValidatorHelper.Validate<GetByIdRequestValidator>(this);
        }
    }
}