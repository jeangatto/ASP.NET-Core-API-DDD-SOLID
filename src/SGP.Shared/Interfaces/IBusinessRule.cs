using System.Threading.Tasks;

namespace SGP.Shared.Interfaces
{
    public interface IBaseBusinessRule
    {
        string Message { get; }
    }

    public interface IBusinessRule : IBaseBusinessRule
    {
        bool IsBroken();
    }

    public interface IBusinessRuleAsync : IBaseBusinessRule
    {
        Task<bool> IsBrokenAsync();
    }
}
