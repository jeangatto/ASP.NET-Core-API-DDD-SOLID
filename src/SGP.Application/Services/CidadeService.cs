using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;

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
            return Result.Invalid(request.ValidationResult.AsErrors());

        var cidade = await _repository.ObterPorIbgeAsync(request.Ibge);
        if (cidade == null)
            return Result.NotFound($"Nenhuma cidade encontrada pelo IBGE: {request.Ibge}");

        return Result.Success(_mapper.Map<CidadeResponse>(cidade));
    }

    public async Task<Result<IEnumerable<CidadeResponse>>> ObterTodosPorUfAsync(ObterTodosPorUfRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return Result.Invalid(request.ValidationResult.AsErrors());

        var cidades = await _repository.ObterTodosPorUfAsync(request.Uf.ToUpperInvariant());
        if (!cidades.Any())
            return Result.NotFound($"Nenhuma cidade encontrada pelo UF: {request.Uf}");

        return Result.Success(_mapper.Map<IEnumerable<CidadeResponse>>(cidades));
    }
}