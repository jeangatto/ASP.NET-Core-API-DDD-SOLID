using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.ValueObjects;
using SGP.Shared.Abstractions;

namespace SGP.Domain.Repositories;

public interface IUsuarioRepository : IAsyncRepository<Usuario>
{
    Task<Usuario> ObterPorEmailAsync(Email email);
    Task<Usuario> ObterPorTokenAtualizacaoAsync(string tokenAtualizacao);
    Task<bool> VerificarSeEmailExisteAsync(Email email);
}