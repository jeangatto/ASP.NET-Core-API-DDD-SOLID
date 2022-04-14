using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
using SGP.Domain.Repositories;

namespace SGP.Application.Services;

public class RegiaoService : IRegiaoService
{
    private readonly IMapper _mapper;
    private readonly IRegiaoRepository _repository;

    public RegiaoService(IMapper mapper, IRegiaoRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<RegiaoResponse>>> ObterTodosAsync()
    {
        var regioes = await _repository.ObterTodosAsync();
        return Result.Ok(_mapper.Map<IEnumerable<RegiaoResponse>>(regioes));
    }
}