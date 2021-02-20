using System;

namespace SGP.Shared.Entities
{
    /// <summary>
    /// Entidade base com o tipo da chave em guid.
    /// </summary>
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}
