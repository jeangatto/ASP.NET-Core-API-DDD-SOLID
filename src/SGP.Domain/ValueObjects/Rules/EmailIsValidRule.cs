using SGP.Shared.Constants;
using SGP.Shared.Interfaces;

namespace SGP.Domain.ValueObjects.Rules
{
    public class EmailIsValidRule : IBusinessRule
    {
        private readonly string _emailAddress;

        public EmailIsValidRule(string emailAddress)
        {
            _emailAddress = emailAddress;
        }

        public string Message => "Endereço de e-mail inválido.";

        public bool IsBroken() => !RegexPatterns.ValidEmailAddress.IsMatch(_emailAddress);
    }
}
