using System;

namespace SGP.Shared.Entities
{
    public abstract class BaseEntity : GuidEntityKey
    {
        protected BaseEntity()
        {
            CadastradoEm = DateTime.Now;
        }

        public DateTime CadastradoEm { get; private set; }
    }
}