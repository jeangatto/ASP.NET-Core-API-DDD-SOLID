using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Requests;
using SGP.Application.Requests.UsuarioRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities.UsuarioAggregate;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
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

        public async Task<Result<CreatedResponse>> AddAsync(AddUsuarioRequest request)
        {
            // Validando a requisição.
            var result = await new AddUsuarioRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros.
                return result.ToFail<CreatedResponse>();
            }

            // Criando o Objeto de Valor (VO).
            var email = Email.Create(request.Email).Value;

            // Criptografando a senha.
            var senhaCriptografada = _hashService.Hash(request.Senha);

            // Criando a instância do usuário.
            var usuario = new Usuario(request.Nome, email, senhaCriptografada);

            // Verificando se o e-mail já existe na base de dados.
            if (await _repository.EmailAlreadyExistsAsync(email))
            {
                return Result.Fail<CreatedResponse>("O endereço de e-mail informado não está disponivel.");
            }

            // Adicionando no repositório.
            _repository.Add(usuario);

            // Confirmando a transação.
            await _uow.SaveChangesAsync();

            // Retornando o resultado.
            var response = new CreatedResponse(usuario.Id);
            return Result.Ok(response);
        }

        public async Task<Result<UsuarioResponse>> GetByIdAsync(GetByIdRequest request)
        {
            // Validando a requisição.
            var result = await new GetByIdRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros.
                return result.ToFail<UsuarioResponse>();
            }

            // Obtendo a entidade do repositório.
            var usuario = await _repository.GetByIdAsync(request.Id);
            if (usuario == null)
            {
                // Retornando erro não encontrado.
                return Result.Fail<UsuarioResponse>($"Registro não encontrado: {request.Id}");
            }

            // Mapeando domínio para resposta (DTO).
            var response = _mapper.Map<UsuarioResponse>(usuario);
            return Result.Ok(response);
        }
    }
}
