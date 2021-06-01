using FluentValidation.Results;
using System;

namespace SGP.Application.Requests.Common
{
    /// <summary>
    /// Classe base usada por solicitações de API.
    /// </summary>
    public abstract class BaseRequest
    {
        protected BaseRequest()
        {
            Timestamp = DateTime.Now;
            ValidationResult = new ValidationResult();
        }

        public DateTime Timestamp { get; }
        public ValidationResult ValidationResult { get; protected set; }

        /// <summary>
        /// Se a requisição é valida.
        /// </summary>
        public bool IsValid => ValidationResult.IsValid;

        /// <summary>
        /// Valida a requisição.
        /// </summary>
        public abstract void Validate();
    }
}