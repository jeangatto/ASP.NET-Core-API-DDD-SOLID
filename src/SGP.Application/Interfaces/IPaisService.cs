using SGP.Application.Responses;
using SGP.Shared.Interfaces;
using SGP.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Interfaces
{
    public interface IPaisService : IService
    {
        Task<IResult<IEnumerable<PaisResponse>>> GetAllAsync();
    }
}
