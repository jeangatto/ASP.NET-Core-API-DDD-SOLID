using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Notifications;
using SGP.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class CidadeService : ICidadeService
    {
        private readonly IMapper _mapper;
        private readonly ICidadeRepository _repository;

        public CidadeService(IMapper mapper, ICidadeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IResult<IEnumerable<CidadeResponse>>> GetAllAsync(GetAllByEstadoRequest request)
        {
            var result = new Result<IEnumerable<CidadeResponse>>();

            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros.
                return result.Fail(request.Notifications);
            }

            // Obtendo as cidades por estado (UF)
            var cidades = await _repository.GetAllAsync(request.EstadoSigla);

            // Mapeando domínio para resposta (DTO).
            var response = _mapper.Map<IEnumerable<CidadeResponse>>(cidades);
            return result.Success(response);
        }

        public async Task<IEnumerable<string>> GetAllEstadosAsync()
        {
            return await _repository.GetAllEstadosAsync();
        }

        public async Task<IResult<CidadeResponse>> GetByIbgeAsync(GetByIbgeRequest request)
        {
            var result = new Result<CidadeResponse>();

            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros.
                return result.Fail(request.Notifications);
            }

            // Obtendo a cidade por IBGE.
            var cidade = await _repository.GetByIbgeAsync(request.Ibge);
            if (cidade == null)
            {
                // Retornando erro de não encontrada.
                return result.Fail(new Notification(nameof(request.Ibge), $"Nenhuma cidade encontrada pelo IBGE: '{request.Ibge}'"));
            }

            // Mapeando domínio para resposta (DTO).
            var response = _mapper.Map<CidadeResponse>(cidade);
            return result.Success(response);
        }
    }
}