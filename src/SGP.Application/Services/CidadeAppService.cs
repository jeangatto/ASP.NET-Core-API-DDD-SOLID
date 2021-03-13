using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Requests.CidadeRequests;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Extensions;
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

        public async Task<IResult<IEnumerable<CidadeResponse>>> GetAllAsync(GetAllByEstadoRequest request)
        {
            request.Validate();
            if (!request.IsValid)
            {
                return Result<IEnumerable<CidadeResponse>>.Fail(request.Notifications);
            }

            var cidades = await _repository.GetAllAsync(request.Estado);
            return _mapper.Map<IEnumerable<CidadeResponse>>(cidades).Success();
        }

        public async Task<IResult<IEnumerable<string>>> GetAllEstadosAsync()
        {
            var estados = await _repository.GetAllEstadosAsync();
            return estados.Success();
        }
    }
}