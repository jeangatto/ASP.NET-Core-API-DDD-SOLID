using System.Collections.Generic;
using SGP.Shared.ValueObjects;

namespace SGP.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private readonly string _emailAddress;

        public Email(string address) => Address = address;

        private Email() // ORM
        {
        }

        public string Address
        {
            get { return _emailAddress; }
            private init { _emailAddress = value?.Trim().ToLowerInvariant(); }
        }

        public override string ToString() => Address;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}