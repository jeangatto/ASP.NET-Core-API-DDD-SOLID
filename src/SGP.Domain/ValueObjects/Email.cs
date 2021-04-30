using FluentResults;
using SGP.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SGP.Domain.ValueObjects
{
    public sealed class Email : ValueObject
    {
        /// <summary>
        /// 320 characters
        /// REF: https://tools.ietf.org/html/rfc3696
        /// </summary>
        public const int MaxLength = 320;
        private static readonly Regex IsValidEmailRegex = new(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);

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
            if (string.IsNullOrWhiteSpace(address))
            {
                return Result.Fail<Email>(new Error("Endereço de e-mail deve ser informado."));
            }

            var length = address.Length;
            if (length > MaxLength)
            {
                return Result.Fail<Email>(new Error($"Endereço de e-mail deve ser menor ou igual a {MaxLength} caracteres. Você digitou {length} caracteres."));
            }

            if (!IsValidEmailRegex.IsMatch(address))
            {
                return Result.Fail<Email>(new Error("Endereço de e-mail inválido."));
            }

            return Result.Ok(new Email(address));
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