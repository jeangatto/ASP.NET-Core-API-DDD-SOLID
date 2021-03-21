using SGP.Shared.Extensions;
using SGP.Shared.Utils;

namespace SGP.Shared.Messages
{
    public abstract class BaseRequestWithValidator<T> : BaseRequest where T : class
    {
        public override void Validate()
        {
            var validator = FluentValidationUtils.GetValidatorInstance<T>();
            if (validator != null)
            {
                validator.Validate(this as T).AddToNotifiable(this);
            }
        }
    }
}