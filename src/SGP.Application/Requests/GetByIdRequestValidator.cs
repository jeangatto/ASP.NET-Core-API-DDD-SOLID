using System;
using FluentValidation;

namespace SGP.Application.Requests
{
    public class GetByIdRequestValidator : AbstractValidator<GetByIdRequest>
    {
        public GetByIdRequestValidator()
        {
            RuleFor(m => m.Id)
                .NotNull()
                .NotEmpty()
                .NotEqual(Guid.Empty);
        }
    }
}