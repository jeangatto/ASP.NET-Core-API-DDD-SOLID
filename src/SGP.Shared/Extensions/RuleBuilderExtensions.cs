using FluentValidation;
using SGP.Shared.Constants;

namespace SGP.Shared.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static IRuleBuilderOptions<T, string> IsValidEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(RegexPatterns.EmailRegexPattern)
                .WithMessage("'{PropertyName}' é um endereço de e-mail inválido.");
        }
    }
}