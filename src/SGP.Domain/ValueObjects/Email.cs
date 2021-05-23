using SGP.Shared.ValueObjects;
using System.Collections.Generic;

namespace SGP.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string address) => Address = address?.ToLowerInvariant();

        private Email() // ORM
        {
        }

        public string Address { get; private init; }

        public override string ToString() => Address;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}