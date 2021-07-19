using SGP.Shared.Exceptions;
using SGP.Shared.Interfaces;
using System.Threading.Tasks;

namespace SGP.Shared.Entities
{
    public abstract class BaseRule
    {
        protected static void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleException(rule);
            }
        }

        protected static async Task CheckRuleAsync(IBusinessRuleAsync rule)
        {
            if (await rule.IsBrokenAsync())
            {
                throw new BusinessRuleException(rule);
            }
        }
    }
}
