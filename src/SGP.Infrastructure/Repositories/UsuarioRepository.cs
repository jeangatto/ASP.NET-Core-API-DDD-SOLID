using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Repositories.Common;

namespace SGP.Infrastructure.Repositories
{
    public class UsuarioRepository : EfRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SgpContext context) : base(context)
        {
        }

        public override Task<Usuario> GetByIdAsync(Guid id, bool readOnly = true)
        {
            return Queryable(readOnly)
                .Include(usuario => usuario.Tokens.OrderByDescending(token => token.ExpiraEm))
                .FirstOrDefaultAsync(usuario => usuario.Id == id);
        }

        public async Task<Usuario> ObterPorEmailAsync(Email email)
        {
            return await Queryable(false)
                .Include(usuario => usuario.Tokens.OrderByDescending(token => token.ExpiraEm))
                .FirstOrDefaultAsync(usuario => usuario.Email.Address == email.Address);
        }

        public async Task<Usuario> ObterPorTokenAtualizacaoAsync(string tokenAtualizacao)
        {
            return await Queryable(false)
                .Include(usuario => usuario.Tokens.Where(token => token.Atualizacao == tokenAtualizacao))
                .Where(usuario => usuario.Tokens.Any(token => token.Atualizacao == tokenAtualizacao))
                .FirstOrDefaultAsync();
        }

        public async Task<bool> VerificarSeEmailExisteAsync(Email email)
            => await Queryable().AnyAsync(usuario => usuario.Email.Address == email.Address);
    }
}