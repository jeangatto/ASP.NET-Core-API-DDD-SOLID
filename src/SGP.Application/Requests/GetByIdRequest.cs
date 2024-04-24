using System;
using System.Threading.Tasks;
using SGP.Shared;
using SGP.Shared.Messages;

namespace SGP.Application.Requests;

public class GetByIdRequest : BaseRequestWithValidation
{
    public GetByIdRequest(Guid id) =>
        Id = id;

    public GetByIdRequest(string id) =>
        Id = Guid.TryParse(id, out var result) ? result : Guid.Empty;

    public Guid Id { get; }

    public override async Task ValidateAsync() =>
        ValidationResult = await LazyValidator.ValidateAsync<GetByIdRequestValidator, GetByIdRequest>(this);
}