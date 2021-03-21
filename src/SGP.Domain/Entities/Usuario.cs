using SGP.Domain.Validators.UsuarioRequires;
using SGP.Domain.ValueObjects;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;
using System;
using System.Collections.Generic;

namespace SGP.Domain.Entities
{
    public class Usuario : BaseEntity, IAggregateRoot
    {
        private readonly List<UsuarioToken> _tokens = new();

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

        public IReadOnlyList<UsuarioToken> Tokens => _tokens.AsReadOnly();

        public void AdicionarToken(UsuarioToken token)
        {
            _tokens.Add(token);
        }

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

        /// <summary>
        /// Incrementa o número de acessos efetuado com sucesso.
        /// </summary>
        public void IncrementarAcessoComSucceso() => AcessosComSucesso++;

        /// <summary>
        /// Incremenenta o número de acessos efetuado com falha.
        /// Quando é atingido o limite de acessos a conta será bloqueada por um tempo.
        /// </summary>
        /// <param name="dateTimeService"></param>
        /// <param name="numeroMaximoTentativas">Número máximo de tentativas até a conta ser bloqueada.</param>
        /// <param name="segundosBloqueado">Segundos em que a conta ficará bloqueada.</param>
        public void IncrementarAcessoComFalha(IDateTimeService dateTimeService, short numeroMaximoTentativas, short segundosBloqueado)
        {
            if (ContaEstaBloqueada(dateTimeService))
            {
                return;
            }

            AcessosComFalha++;

            if (AcessosComFalha == numeroMaximoTentativas)
            {
                AcessosComFalha = 0;
                DataBloqueio = dateTimeService.Now.AddSeconds(segundosBloqueado);
            }
        }
    }
}
