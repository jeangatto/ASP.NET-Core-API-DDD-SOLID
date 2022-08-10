using System;
using SGP.Shared.Entities;
using SGP.Shared.Interfaces;

namespace SGP.Domain.Entities;

public class Token : BaseEntity
{
    public Token(string acesso, string atualizacao, DateTime criadoEm, DateTime expiraEm)
    {
        Acesso = acesso;
        Atualizacao = atualizacao;
        CriadoEm = criadoEm;
        ExpiraEm = expiraEm;
    }

    private Token() // ORM
    {
    }

    /// <summary>
    /// Identificação do usuário.
    /// </summary>
    public Guid UsuarioId { get; private init; }

    /// <summary>
    /// Token de acesso (AccessToken), utilizado para acessar o sistema.
    /// </summary>
    public string Acesso { get; private init; }

    /// <summary>
    /// Token de atualização (RefreshToken), utilizado para gerar um novo token.
    /// </summary>
    public string Atualizacao { get; private init; }

    /// <summary>
    /// Data da criação do Token.
    /// </summary>
    public DateTime CriadoEm { get; private init; }

    /// <summary>
    /// Data do vencimento do token.
    /// </summary>
    public DateTime ExpiraEm { get; private init; }

    /// <summary>
    /// Data da revogação (cancelamento) do token.
    /// </summary>
    public DateTime? RevogadoEm { get; private set; }

    public Usuario Usuario { get; private init; }

    /// <summary>
    /// Indica se o Token foi revogado (cancelado).
    /// </summary>
    public bool EstaRevogado => RevogadoEm.HasValue;

    /// <summary>
    /// Indica se o token está expirado ou revogado.
    /// </summary>
    /// <param name="dateTimeService"></param>
    /// <returns>Verdadeiro se o token estiver expirado ou revogado; caso contrário, falso.</returns>
    public bool EstaValido(IDateTimeService dateTimeService) => ExpiraEm >= dateTimeService.Now || EstaRevogado;

    /// <summary>
    /// Revoga (cancela) o token.
    /// </summary>
    /// <param name="dataRevogacao">Data da revogação.</param>
    public void Revogar(DateTime dataRevogacao)
    {
        if (!EstaRevogado)
            RevogadoEm = dataRevogacao;
    }
}