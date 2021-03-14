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
    public class CidadeAppService : ICidadeAppService
    {
        private readonly IMapper _mapper;
        private readonly ICidadeRepository _repository;

        public CidadeAppService(IMapper mapper, ICidadeRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IResult<IEnumerable<CidadeResponse>>> GetAllAsync(GetAllByEstadoRequest req)
        {
            var result = new Result<IEnumerable<CidadeResponse>>();

            req.Validate();
            if (!req.IsValid)
            {
                return result.Fail(req.Notifications);
            }

            var cidades = await _repository.GetAllAsync(req.Estado);
            var response = _mapper.Map<IEnumerable<CidadeResponse>>(cidades);
            return result.Success(response);
        }

        public async Task<IEnumerable<string>> GetAllEstadosAsync()
        {
            return await _repository.GetAllEstadosAsync();
        }

        public async Task<IResult<CidadeResponse>> GetByIbgeAsync(GetByIbgeRequest req)
        {
            var result = new Result<CidadeResponse>();

            req.Validate();
            if (!req.IsValid)
            {
                return result.Fail(req.Notifications);
            }

            var cidade = await _repository.GetByIbgeAsync(req.Ibge);
            if (cidade == null)
            {
                return result.Fail(new Notification(nameof(req.Ibge), $"Nenhuma cidade encontrada pelo IBGE: '{req.Ibge}'"));
            }

            var response = _mapper.Map<CidadeResponse>(cidade);
            return result.Success(response);
        }
    }
}