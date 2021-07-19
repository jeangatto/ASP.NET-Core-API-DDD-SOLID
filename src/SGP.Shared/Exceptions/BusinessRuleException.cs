using SGP.Shared.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;

namespace SGP.Shared.Exceptions
{
    [SuppressMessage("Design", "RCS1194:Implement exception constructors.")]
    [SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly")]
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(IBaseBusinessRule brokenRule) : base(brokenRule.Message)
        {
            BrokenRule = brokenRule;
            Details = brokenRule.Message;
        }

        public IBaseBusinessRule BrokenRule { get; }
        public string Details { get; }

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
        }
    }
}
