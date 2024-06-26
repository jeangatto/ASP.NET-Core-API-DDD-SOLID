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

public class EstadoService(IMapper mapper, IEstadoRepository estadoRepository) : IEstadoService
{
    public async Task<Result<IEnumerable<EstadoResponse>>> ObterTodosAsync()
    {
        var estados = await estadoRepository.ObterTodosAsync();
        return Result.Success(mapper.Map<IEnumerable<EstadoResponse>>(estados));
    }

    public async Task<Result<IEnumerable<EstadoResponse>>> ObterTodosPorRegiaoAsync(ObterTodosPorRegiaoRequest request)
    {
        await request.ValidateAsync();
        if (!request.IsValid)
            return Result.Invalid(request.ValidationResult.AsErrors());

        var estados = await estadoRepository.ObterTodosPorRegiaoAsync(request.Regiao);
        return !estados.Any()
            ? Result.NotFound($"Nenhum estado encontrado pela região: {request.Regiao}")
            : Result.Success(mapper.Map<IEnumerable<EstadoResponse>>(estados));
    }
}