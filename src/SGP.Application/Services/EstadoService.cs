using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Responses;
using SGP.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class EstadoService : IEstadoService
    {
        private readonly IMapper _mapper;
        private readonly IEstadoRepository _repository;

        public EstadoService(IMapper mapper, IEstadoRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<EstadoResponse>> ObterTodosAsync()
        {
            var estados = await _repository.ObterTodosAsync();
            return _mapper.Map<IEnumerable<EstadoResponse>>(estados);
        }
    }
}
