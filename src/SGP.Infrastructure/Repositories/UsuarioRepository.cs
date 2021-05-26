using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities.UsuarioAggregate;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories.Common;
using System.Linq;
using System.Threading.Tasks;

namespace SGP.Infrastructure.Repositories
{
    public class UsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SgpContext context) : base(context)
        {
        }

        public async Task<Usuario> ObterPorEmailAsync(Email email)
        {
            return await Queryable(false)
                .Include(u => u.Tokens)
                .FirstOrDefaultAsync(u => u.Email.Address == email.Address);
        }

        public async Task<Usuario> ObterPorTokenAsync(string token)
        {
            return await Queryable()
               .Include(u => u.Tokens.Any(t => t.Token == token))
               .FirstOrDefaultAsync();
        }

        public async Task<bool> VerificaSeEmailExisteAsync(Email email)
        {
            return await Queryable()
                .AnyAsync(u => u.Email.Address == email.Address);
        }
    }
}
