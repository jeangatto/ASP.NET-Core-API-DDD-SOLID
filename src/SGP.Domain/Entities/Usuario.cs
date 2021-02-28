using SGP.Domain.Validators.UsuarioRequires;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Domain.Entities
{
    public class Usuario : BaseEntity, IAggregateRoot
    {
        public Usuario(string nome, string email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Validate(this, new NovoUsuarioValidator());
        }

        private Usuario()
        {
        }

        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime? DataUltimoAcesso { get; private set; }
        public DateTime? DataBloqueio { get; private set; }
        public short AcessosComSucesso { get; private set; }
        public short AcessosComFalha { get; private set; }

        /// <summary>
        /// Indica que a conta do usuário está bloqueada.
        /// </summary>
        public bool ContaBloqueada => DataBloqueio.HasValue && DataBloqueio.Value > DateTime.Now;

        public void AlterarNome(string nome)
        {
            Nome = nome;
            Validate(this, new AlterarNomeValidator());
        }

        public void AlterarEmail(string email)
        {
            Email = email;
            Validate(this, new AlterarEmailValidator());
        }

        public void AlterarSenha(string senha)
        {
            Senha = senha;
            Validate(this, new AlterarSenhaValidator());
        }

        public void AtualizarDataUltimoAcesso()
        {
            DataUltimoAcesso = DateTime.Now;
        }
    }
}
