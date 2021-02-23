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
            CadastradoEm = DateTime.Now;
        }

        /// <summary>
        /// Chave (ID).
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Data da criação da entidade.
        /// </summary>
        public DateTime CadastradoEm { get; private set; }
    }
}
