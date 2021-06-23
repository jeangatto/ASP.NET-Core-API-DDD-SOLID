using FluentValidation;
using System.Text.RegularExpressions;

namespace SGP.Shared.Extensions
{
    public static class RuleBuilderExtensions
    {
        private static readonly Regex IsValidEmailRegex = new(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static IRuleBuilderOptions<T, string> IsValidEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(IsValidEmailRegex)
                .WithMessage("'{PropertyName}' é um endereço de e-mail inválido.");
        }
    }
}