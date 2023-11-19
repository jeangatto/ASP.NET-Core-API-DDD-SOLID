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
    private readonly IMapper _mapper = mapper;
    private readonly IRegiaoRepository _repository = repository;

    public async Task<Result<IEnumerable<RegiaoResponse>>> ObterTodosAsync()
    {
        var regioes = await _repository.ObterTodosAsync();
        return Result.Success(_mapper.Map<IEnumerable<RegiaoResponse>>(regioes));
    }
}