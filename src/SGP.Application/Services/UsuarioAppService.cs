using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Requests;
using SGP.Application.Requests.UsuarioRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.Interfaces;
using SGP.Shared.Messages;
using SGP.Shared.Notifications;
using SGP.Shared.Results;
using SGP.Shared.UnitOfWork;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IHashService _hashService;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;

        public UsuarioAppService
        (
            IHashService hashService,
            IMapper mapper,
            IUsuarioRepository repository,
            IUnitOfWork uow
        )
        {
            _hashService = hashService;
            _mapper = mapper;
            _repository = repository;
            _uow = uow;
        }

        public async Task<IResult<CreatedResponse>> AddAsync(AddUsuarioRequest req)
        {
            var result = new Result<CreatedResponse>();

            // Validando a requisição.
            req.Validate();
            if (!req.IsValid)
            {
                // Retornando os erros.
                return result.Fail(req.Notifications);
            }

            var email = new Email(req.Email);
            var usuario = new Usuario(req.Nome, email, _hashService.Hash(req.Senha));

            // Validando a entidade de domínio.
            if (!usuario.IsValid)
            {
                // Retornando os erros.
                return result.Fail(usuario.Notifications);
            }

            // Verificando se o e-mail já existe na base de dados.
            if (await _repository.EmailAlreadyExistsAsync(email))
            {
                return result.Fail(new Notification(nameof(req.Email), "O endereço de e-mail informado não está disponivel."));
            }

            // Adicionando no repositório.
            _repository.Add(usuario);

            // Confirmando a transação.
            await _uow.SaveChangesAsync();

            // Retornando o resultado.
            return result.Success(new CreatedResponse(usuario.Id));
        }

        public async Task<IResult<UsuarioResponse>> GetByIdAsync(GetByIdRequest req)
        {
            var result = new Result<UsuarioResponse>();

            // Validando a requisição.
            req.Validate();
            if (!req.IsValid)
            {
                // Retornando os erros.
                return result.Fail(req.Notifications);
            }

            // Obtendo a entidade do repositório.
            var usuario = await _repository.GetByIdAsync(req.Id);
            if (usuario == null)
            {
                // Retornando erro não encontrado.
                return result.Fail(new Notification(nameof(req.Id), $"Registro não encontrado: {req.Id}."));
            }

            // Mapeando domínio para resposta (DTO).
            var response = _mapper.Map<UsuarioResponse>(usuario);
            return result.Success(response);
        }
    }
}
