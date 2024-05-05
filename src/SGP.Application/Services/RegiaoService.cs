using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
using SGP.Domain.Repositories;

namespace SGP.Application.Services;

public class RegiaoService(IMapper mapper, IRegiaoRepository repository) : IRegiaoService
{
    public async Task<Result<IEnumerable<RegiaoResponse>>> ObterTodosAsync()
    {
        var regioes = await repository.ObterTodosAsync();
        return Result.Success(mapper.Map<IEnumerable<RegiaoResponse>>(regioes));
    }
}