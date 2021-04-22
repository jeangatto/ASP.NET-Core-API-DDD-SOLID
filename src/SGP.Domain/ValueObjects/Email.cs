using FluentResults;
using SGP.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SGP.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        private static readonly Regex IsValidEmailRegex = new(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private Email(string address)
        {
            Address = address.ToLowerInvariant();
        }

        private Email() // ORM
        {
        }

        public string Address { get; private init; }

        public static Result<Email> Create(string address)
        {
            if (!string.IsNullOrWhiteSpace(address) && IsValidEmailRegex.IsMatch(address))
            {
                return Result.Ok(new Email(address));
            }
            else
            {
                return Result.Fail<Email>(new Error("Endereço de e-mail inválido."));
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Address;
        }

        public override string ToString()
        {
            return Address;
        }
    }
}