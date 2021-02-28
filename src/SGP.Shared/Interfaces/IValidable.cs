using FluentValidation;

namespace SGP.Shared.Interfaces
{
    public interface IValidable
    {
        void Validate<T>(T instance, IValidator<T> validator) where T : class;
    }
}
