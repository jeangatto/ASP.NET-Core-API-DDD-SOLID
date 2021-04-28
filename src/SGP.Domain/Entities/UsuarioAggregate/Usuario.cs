using SGP.Domain.ValueObjects;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace SGP.Domain.Entities.UsuarioAggregate
{
    public class Usuario : BaseEntity, IAggregateRoot
    {
        private readonly List<RefreshToken> _refreshTokens = new();

        public Usuario(string nome, Email email, string senha)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
        }

        private Usuario() // ORM
        {
        }

        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public string Senha { get; private set; }
        public DateTime? DataUltimoAcesso { get; private set; }
        public DateTime? DataBloqueio { get; private set; }
        public short AcessosComSucesso { get; private set; }
        public short AcessosComFalha { get; private set; }

        public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        /// <summary>
        /// Indica se a conta do usuário está bloqueada.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns>Verdadeiro se a conta estiver bloqueada; caso contrário, falso.</returns>
        public bool ContaEstaBloqueada(IDateTime dateTime)
        {
            return DataBloqueio > dateTime.Now;
        }

        /// <summary>
        /// Adiciona um novo token de acesso para o usuário.
        /// </summary>
        /// <param name="refreshToken"></param>
        public void AdicionarRefreshToken(RefreshToken refreshToken)
        {
            _refreshTokens.Add(refreshToken);
        }

        public void AlterarNome(string nome)
        {
            Nome = nome;
        }

        public void AlterarEmail(Email email)
        {
            Email = email;
        }

        public void AlterarSenha(string senha)
        {
            Senha = senha;
        }

        public void AtualizarDataUltimoAcesso(IDateTime dateTime)
        {
            DataUltimoAcesso = dateTime.Now;
        }

        /// <summary>
        /// Incrementa o número de acessos efetuado com sucesso.
        /// </summary>
        public void IncrementarAcessoComSucceso() => AcessosComSucesso++;

        /// <summary>
        /// Incremenenta o número de acessos efetuado com falha.
        /// Quando é atingido o limite de acessos a conta será bloqueada por um tempo.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="numeroMaximoTentativas">Número máximo de tentativas até a conta ser bloqueada.</param>
        /// <param name="segundosBloqueado">Segundos em que a conta ficará bloqueada.</param>
        public void IncrementarAcessoComFalha(IDateTime dateTime, short numeroMaximoTentativas, short segundosBloqueado)
        {
            if (ContaEstaBloqueada(dateTime))
            {
                return;
            }

            AcessosComFalha++;

            if (AcessosComFalha == numeroMaximoTentativas)
            {
                AcessosComFalha = 0;
                DataBloqueio = dateTime.Now.AddSeconds(segundosBloqueado);
            }
        }
    }
}
