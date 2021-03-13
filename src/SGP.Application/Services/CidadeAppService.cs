using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
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
            return result.Success(_mapper.Map<IEnumerable<CidadeResponse>>(cidades));
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
                return result.Fail($"Nenhuma cidade encontrada pelo ibge {req.Ibge}.");
            }

            return result.Success(_mapper.Map<CidadeResponse>(cidade));
        }
    }
}