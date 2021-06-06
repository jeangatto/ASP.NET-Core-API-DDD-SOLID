using FluentValidation.Results;

namespace SGP.Shared.Messages
{
    /// <summary>
    /// Classe base usada por solicitações da API.
    /// </summary>
    public abstract class BaseRequest
    {
        protected BaseRequest()
        {
            ValidationResult = new ValidationResult();
        }

        public ValidationResult ValidationResult { get; protected set; }

        /// <summary>
        /// Indica se a requisição é valida.
        /// </summary>
        public bool IsValid => ValidationResult.IsValid;

        /// <summary>
        /// Valida a requisição.
        /// </summary>
        public abstract void Validate();
    }
}