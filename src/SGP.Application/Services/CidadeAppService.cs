using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class CidadeAppService : ICidadeAppService
    {
        private readonly IMapper _mapper;
        private readonly ICidadeRepository _repository;

        public CidadeAppService(IMapper mapper, ICidadeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Result<IEnumerable<CidadeResponse>>> GetAllAsync(GetAllByEstadoRequest request)
        {
            // Validando a requisição.
            var result = await new GetAllByEstadoRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros.
                return result.ToFail<IEnumerable<CidadeResponse>>();
            }

            // Obtendo as cidades por estado (UF)
            var cidades = await _repository.GetAllAsync(request.EstadoSigla);

            // Mapeando domínio para resposta (DTO).
            var response = _mapper.Map<IEnumerable<CidadeResponse>>(cidades);
            return Result.Ok(response);
        }

        public async Task<IEnumerable<string>> GetAllEstadosAsync()
        {
            return await _repository.GetAllEstadosAsync();
        }

        public async Task<Result<CidadeResponse>> GetByIbgeAsync(GetByIbgeRequest request)
        {
            // Validando a requisição.
            var result = await new GetByIbgeRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros.
                return result.ToFail<CidadeResponse>();
            }

            // Obtendo a cidade por IBGE.
            var cidade = await _repository.GetByIbgeAsync(request.Ibge);
            if (cidade == null)
            {
                // Não encontrado.
                return Result.Fail<CidadeResponse>($"Nenhuma cidade encontrada pelo IBGE: '{request.Ibge}'");
            }

            // Mapeando domínio para resposta (DTO).
            var response = _mapper.Map<CidadeResponse>(cidade);
            return Result.Ok(response);
        }
    }
}