using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using SGP.Shared.Extensions;
using SGP.Shared.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class PaisService : IPaisService
    {
        private readonly IMapper _mapper;
        private readonly IPaisRepository _repository;

        public PaisService(IMapper mapper, IPaisRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IResult<IEnumerable<PaisResponse>>> GetAllAsync()
        {
            var paises = await _repository.GetAllAsync();
            var response = _mapper.Map<IEnumerable<PaisResponse>>(paises);
            return response.Success();
        }
    }
}