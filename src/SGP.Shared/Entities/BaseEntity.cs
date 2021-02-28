using FluentValidation;
using SGP.Shared.Interfaces;
using SGP.Shared.Notifications;
using System;

namespace SGP.Shared.Entities
{
    public abstract class BaseEntity : GuidEntityKey
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public void Validate<T>(T instance, IValidator<T> validator) where T : class
        {
            AddNotifications(validator.Validate(instance));
        }
    }
}