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

        /// <summary>
        /// Valida a entidade e se no resultado da validação existirem erros, eles serão adicionados nas notificações.
        /// </summary>
        /// <typeparam name="T">O tipo de entidade que será validado.</typeparam>
        /// <param name="instance">A instância da entidade.</param>
        /// <param name="validator">O validador da entidade.</param>
        public void Validate<T>(T instance, IValidator<T> validator) where T : class
        {
            validator.Validate(instance).AddToNotifiable(this);
        }
    }
}