using SGP.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface ICidadeAppService
    {
        Task<IResult<IEnumerable<string>>> GetAllEstadosAsync();
    }
}
