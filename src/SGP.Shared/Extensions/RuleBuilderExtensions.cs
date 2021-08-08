using FluentValidation;
using SGP.Shared.Constants;

namespace SGP.Shared.Extensions
{
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Defines an email validator on the current rule builder for string properties.
        /// Validation will fail if the value returned by the lambda is not a valid email address.
        /// </summary>
        /// <typeparam name="T">Type of object being validated</typeparam>
        /// <param name="ruleBuilder">The rule builder on which the validator should be defined</param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> IsValidEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Matches(RegexPatterns.EmailRegexPattern)
                .WithMessage("'{PropertyName}' é um endereço de e-mail inválido.");
        }
    }
}