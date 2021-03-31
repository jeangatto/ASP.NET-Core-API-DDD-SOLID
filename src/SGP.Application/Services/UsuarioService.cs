using AutoMapper;
using SGP.Application.Interfaces;
using SGP.Application.Requests;
using SGP.Application.Requests.UsuarioRequests;
using SGP.Application.Responses;
using SGP.Application.Responses.Common;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.Interfaces;
using SGP.Shared.Notifications;
using SGP.Shared.Results;
using SGP.Shared.UnitOfWork;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IHashService _hashService;
        private readonly IMapper _mapper;
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _uow;

        public UsuarioService
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

        public async Task<IResult<CreatedResponse>> AddAsync(AddUsuarioRequest request)
        {
            var result = new Result<CreatedResponse>();

            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros.
                return result.Fail(request.Notifications);
            }

            var senhaCriptografada = _hashService.Hash(request.Senha);
            var email = new Email(request.Email);
            var usuario = new Usuario(request.Nome, email, senhaCriptografada);

            // Validando a entidade de domínio.
            if (!usuario.IsValid)
            {
                // Retornando os erros.
                return result.Fail(usuario.Notifications);
            }

            // Verificando se o e-mail já existe na base de dados.
            if (await _repository.EmailAlreadyExistsAsync(email))
            {
                return result.Fail(new Notification(nameof(request.Email), "O endereço de e-mail informado não está disponivel."));
            }

            // Adicionando no repositório.
            _repository.Add(usuario);

            // Confirmando a transação.
            await _uow.SaveChangesAsync();

            // Retornando o resultado.
            return result.Success(new CreatedResponse(usuario.Id));
        }

        public async Task<IResult<UsuarioResponse>> GetByIdAsync(GetByIdRequest request)
        {
            var result = new Result<UsuarioResponse>();

            // Validando a requisição.
            request.Validate();
            if (!request.IsValid)
            {
                // Retornando os erros.
                return result.Fail(request.Notifications);
            }

            // Obtendo a entidade do repositório.
            var usuario = await _repository.GetByIdAsync(request.Id);
            if (usuario == null)
            {
                // Retornando erro não encontrado.
                return result.Fail(new Notification(nameof(request.Id), $"Registro não encontrado: {request.Id}."));
            }

            // Mapeando domínio para resposta (DTO).
            var response = _mapper.Map<UsuarioResponse>(usuario);
            return result.Success(response);
        }
    }
}
