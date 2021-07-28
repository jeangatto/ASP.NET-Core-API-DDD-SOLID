using System;
using SGP.Shared.Interfaces;

namespace SGP.Shared.Entities
{
    /// <summary>
    /// Entidade com a chave (PK) tipada em <see cref="Guid"/>.
    /// </summary>
    public abstract class BaseEntity : BaseValidator, IEntityKey<Guid>
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}