using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Requests.EstadoRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;

namespace SGP.Application.Services;

public class EstadoService : IEstadoService
{
    private readonly IMapper _mapper;
    private readonly IEstadoRepository _repository;

    public EstadoService(IMapper mapper, IEstadoRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<Result<IEnumerable<EstadoResponse>>> ObterTodosAsync()
    {
        var estados = await _repository.ObterTodosAsync();
        return Result.Ok(_mapper.Map<IEnumerable<EstadoResponse>>(estados));
    }

    public async Task<Result<IEnumerable<EstadoResponse>>> ObterTodosPorRegiaoAsync(ObterTodosPorRegiaoRequest request)
    {
        // Validando a requisição.
        await request.ValidateAsync();
        if (!request.IsValid)
        {
            // Retornando os erros da validação.
            return request.ToFail<IEnumerable<EstadoResponse>>();
        }

        // Obtendo as cidades pelo UF.
        var estados = await _repository.ObterTodosPorRegiaoAsync(request.Regiao);
        if (!estados.Any())
        {
            // Retornando não encontrado.
            return Result.Fail<IEnumerable<EstadoResponse>>(
                new NotFoundError($"Nenhum estado encontrado pela região: {request.Regiao}"));
        }

        // Mapeando domínio para resposta (DTO).
        return Result.Ok(_mapper.Map<IEnumerable<EstadoResponse>>(estados));
    }
}