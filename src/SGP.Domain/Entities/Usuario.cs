using SGP.Shared.Entities;
using System;

namespace SGP.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public Usuario(string apelido, string email, string senha)
        {
            Apelido = apelido;
            Email = email;
            Senha = senha;
        }

        private Usuario()
        {
        }

        public string Apelido { get; private set; }
        public string Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime UltimoAcessoEm { get; private set; }
        public DateTime? DataBloqueio { get; private set; }
        public short AcessosComSucesso { get; private set; }
        public short AcessosComFalha { get; private set; }

        public bool ContaBloqueada => DataBloqueio.HasValue && DataBloqueio > DateTime.Now;

        public void AlterarApelido(string apelido)
        {
            Apelido = apelido;
        }

        public void AlterarEmail(string email)
        {
            Email = email;
        }

        public void AlterarSenha(string senha)
        {
            Senha = senha;
        }

        public void AtualizarDataUltimoAcesso()
        {
            UltimoAcessoEm = DateTime.Now;
        }
    }
}
