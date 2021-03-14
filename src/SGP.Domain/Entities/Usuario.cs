using SGP.Domain.Validators.UsuarioRequires;
using SGP.Domain.ValueObjects;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;

namespace SGP.Domain.Entities
{
    public class Usuario : BaseEntity, IAggregateRoot
    {
        public Usuario(string nome, Email email, string senha)
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
        public Email Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime? DataUltimoAcesso { get; private set; }
        public DateTime? DataBloqueio { get; private set; }
        public short AcessosComSucesso { get; private set; }
        public short AcessosComFalha { get; private set; }

        /// <summary>
        /// Indica se a conta do usuário está bloqueada.
        /// </summary>
        /// <param name="dateTimeService"></param>
        /// <returns>Verdadeiro se a conta estiver bloqueada; caso contrário, falso.</returns>
        public bool ContaEstaBloqueada(IDateTimeService dateTimeService)
        {
            return DataBloqueio > dateTimeService.Now;
        }

        public void AlterarNome(string nome)
        {
            Nome = nome;
            Validate(this, new AlterarNomeValidator());
        }

        public void AlterarEmail(Email email)
        {
            Email = email;
            Validate(this, new AlterarEmailValidator());
        }

        public void AlterarSenha(string senha)
        {
            Senha = senha;
            Validate(this, new AlterarSenhaValidator());
        }

        public void AtualizarDataUltimoAcesso(IDateTimeService dateTimeService)
        {
            DataUltimoAcesso = dateTimeService.Now;
        }
    }
}
