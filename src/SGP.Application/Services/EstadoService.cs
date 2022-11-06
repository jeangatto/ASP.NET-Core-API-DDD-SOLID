using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Requests.EstadoRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;

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
        return Result.Success(_mapper.Map<IEnumerable<EstadoResponse>>(estados));
    }

    public async Task<Result<IEnumerable<EstadoResponse>>> ObterTodosPorRegiaoAsync(ObterTodosPorRegiaoRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return Result.Invalid(request.ValidationResult.AsErrors());

        var estados = await _repository.ObterTodosPorRegiaoAsync(request.Regiao);
        if (!estados.Any())
            return Result.NotFound($"Nenhum estado encontrado pela região: {request.Regiao}");

        return Result.Success(_mapper.Map<IEnumerable<EstadoResponse>>(estados));
    }
}