using AutoMapper;
using FluentResults;
using SGP.Application.Interfaces;
using SGP.Application.Requests;
using SGP.Application.Requests.UserRequests;
using SGP.Application.Responses;
using SGP.Domain.Entities.UserAggregate;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.Errors;
using SGP.Shared.Extensions;
using SGP.Shared.Interfaces;
using SGP.Shared.UnitOfWork;
using System.Threading.Tasks;

namespace SGP.Application.Services
{
    public class UserAppService : IUserAppService
    {
        private readonly IHashService _hashService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _uow;

        public UserAppService
        (
            IHashService hashService,
            IMapper mapper,
            IUserRepository repository,
            IUnitOfWork uow
        )
        {
            _hashService = hashService;
            _mapper = mapper;
            _repository = repository;
            _uow = uow;
        }

        public async Task<Result<CreatedResponse>> CreateAsync(CreateUserRequest request)
        {
            // Validando a requisição.
            var result = await new CreateUserRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros da validação.
                return result.ToFail<CreatedResponse>();
            }

            // Criando o Value Object.
            var email = new Email(request.Email);

            // Verificando se o e-mail já existe na base de dados.
            if (await _repository.EmailAlreadyExistsAsync(email))
            {
                return Result.Fail<CreatedResponse>("O endereço de e-mail informado não está disponivel.");
            }

            // Criptografando a senha.
            var passwordHash = _hashService.Hash(request.Password);

            // Criando a instância do usuário.
            var user = new User(request.Name, email, passwordHash);

            // Adicionando no repositório.
            _repository.Add(user);

            // Confirmando a transação.
            await _uow.SaveChangesAsync();

            // Retornando o resultado.
            return Result.Ok(new CreatedResponse(user.Id));
        }

        public async Task<Result<UserResponse>> GetByIdAsync(GetByIdRequest request)
        {
            // Validando a requisição.
            var result = await new GetByIdRequestValidator().ValidateAsync(request);
            if (!result.IsValid)
            {
                // Retornando os erros da validação.
                return result.ToFail<UserResponse>();
            }

            // Obtendo a entidade do repositório.
            var user = await _repository.GetByIdAsync(request.Id);
            if (user == null)
            {
                // Retornando não encontrado.
                return Result.Fail<UserResponse>(
                    new NotFoundError($"Registro não encontrado: {request.Id}"));
            }

            // Mapeando domínio para resposta (DTO).
            return Result.Ok(_mapper.Map<UserResponse>(user));
        }
    }
}
