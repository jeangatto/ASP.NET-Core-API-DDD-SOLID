using SGP.Domain.ValueObjects.Rules;
using SGP.Shared.ValueObjects;
using System.Collections.Generic;

namespace SGP.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private Email(string address)
        {
            Address = address?.ToLowerInvariant();
        }

        private Email() // ORM
        {
        }

        public string Address { get; private init; }

        public static Email Create(string address)
        {
            CheckRule(new EmailIsValidRule(address));
            return new Email(address);
        }

        public override string ToString()
        {
            return Address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}