using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Shared.Interfaces;
using System.Threading.Tasks;

namespace SGP.Domain.Entities.Rules
{
    public class UsuarioVerificarDisponibilidadeEmailRule : IBusinessRuleAsync
    {
        private readonly Email _email;
        private readonly IUsuarioRepository _repository;

        public UsuarioVerificarDisponibilidadeEmailRule(Email email, IUsuarioRepository repository)
        {
            _email = email;
            _repository = repository;
        }

        public string Message => "O endereço de e-mail informado já está sendo utilizado.";

        public async Task<bool> IsBrokenAsync() => await _repository.VerificaSeEmailExisteAsync(_email);
    }
}
