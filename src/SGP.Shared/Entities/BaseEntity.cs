using FluentValidation;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
using SGP.Shared.Notifications;
using System;

namespace SGP.Shared.Entities
{
    /// <summary>
    /// Entidade com a chave (PK) tipada em <see cref="Guid"/>.
    /// </summary>
    public abstract class BaseEntity : Notifiable, IEntityKey<Guid>
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public void Validate<T>(T instance, IValidator<T> validator) where T : class
        {
            validator.Validate(instance).AddToNotifiable(this);
        }
    }
}