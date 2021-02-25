using SGP.Shared.Interfaces;
using System;

namespace SGP.Shared.Entities
{
    public abstract class GuidEntityKey : IEntityKey<Guid>
    {
        protected GuidEntityKey()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
    }
}
