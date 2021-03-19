using SGP.Shared.Extensions;
using SGP.Shared.Utils;

namespace SGP.Shared.Messages
{
    public abstract class BaseRequestWithValidator<T> : BaseRequest where T : class
    {
        private readonly bool _cacheValidator;

        protected BaseRequestWithValidator(bool cacheValidator = false)
        {
            _cacheValidator = cacheValidator;
        }

        public override void Validate()
        {
            var validator = FluentValidationUtils.GetValidatorInstance<T>(_cacheValidator);
            if (validator != null)
            {
                validator.Validate(this as T).AddToNotifiable(this);
            }
        }
    }
}