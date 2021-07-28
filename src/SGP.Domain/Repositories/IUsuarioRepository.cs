using System.Threading.Tasks;
using SGP.Domain.Entities;
using SGP.Domain.ValueObjects;
using SGP.Shared.Interfaces;

namespace SGP.Domain.Repositories
{
    public interface IUsuarioRepository : IAsyncRepository<Usuario>
    {
        Task<Usuario> ObterPorEmailAsync(Email email);
        Task<Usuario> ObterPorTokenAsync(string token);
        Task<bool> VerificarSeEmailExisteAsync(Email email);
    }
}