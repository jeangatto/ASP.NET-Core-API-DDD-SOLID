using AutoMapper;
using SGP.Application.Interfaces;
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

        public async Task<IResult<IEnumerable<string>>> GetAllEstadosAsync()
        {
            var estados = await _repository.GetAllEstadosAsync();
            return estados.Success();
        }
    }
}