using SGP.Shared.Results;
using SGP.Shared.ValueObjects;
using System.Collections.Generic;
using System.Net.Mail;

namespace SGP.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private Email(string address)
        {
            Address = address.ToLowerInvariant();
        }

        private Email() // ORM
        {
        }

        public string Address { get; private set; }

        public static Result<Email> Create(string address)
        {
            if (MailAddress.TryCreate(address, out MailAddress mailAddress))
            {
                return Result.Success(new Email(mailAddress.Address));
            }
            else
            {
                return Result.Failure<Email>("Endereço de e-mail inválido.");
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }
    }
}