using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using SGP.Application.Responses;
using SGP.Shared.Interfaces;

namespace SGP.Application.Interfaces;

public interface IRegiaoService : IAppService
{
    Task<Result<IEnumerable<RegiaoResponse>>> ObterTodosAsync();
}