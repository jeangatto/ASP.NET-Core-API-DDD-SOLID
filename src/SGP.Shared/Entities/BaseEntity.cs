using System;

namespace SGP.Shared.Entities
{
    /// <summary>
    /// Entidade base com a chave tipada em <see cref="Guid"/>.
    /// </summary>
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Chave (ID).
        /// </summary>
        public Guid Id { get; private set; }
    }
}
