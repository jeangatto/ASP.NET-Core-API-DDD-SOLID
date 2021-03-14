using SGP.Shared.ValueObjects;
using System.Collections.Generic;

namespace SGP.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public Email(string address)
        {
            Address = address?.ToLowerInvariant();
        }

        public string Address { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}