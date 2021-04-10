using FluentValidation;
using System;

namespace SGP.Application.Requests
{
    public class GetByIdRequestValidator : AbstractValidator<GetByIdRequest>
    {
        public GetByIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}