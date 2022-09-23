using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;

namespace SGP.Application.Services;

public class CidadeService : ICidadeService
{
    private readonly IMapper _mapper;
    private readonly ICidadeRepository _repository;

    public CidadeService(IMapper mapper, ICidadeRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<CidadeResponse>> ObterPorIbgeAsync(ObterPorIbgeRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return request.ToFail<CidadeResponse>();

        var cidade = await _repository.ObterPorIbgeAsync(request.Ibge);
        if (cidade == null)
        {
            return Result.Fail<CidadeResponse>(
                new NotFoundError($"Nenhuma cidade encontrada pelo IBGE: {request.Ibge}"));
        }

        return Result.Ok(_mapper.Map<CidadeResponse>(cidade));
    }

    public async Task<Result<IEnumerable<CidadeResponse>>> ObterTodosPorUfAsync(ObterTodosPorUfRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return request.ToFail<IEnumerable<CidadeResponse>>();

        var cidades = await _repository.ObterTodosPorUfAsync(request.Uf.ToUpperInvariant());
        if (!cidades.Any())
        {
            return Result.Fail<IEnumerable<CidadeResponse>>(
                new NotFoundError($"Nenhuma cidade encontrada pelo UF: {request.Uf}"));
        }

        return Result.Ok(_mapper.Map<IEnumerable<CidadeResponse>>(cidades));
    }
}