using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Context;
using SGP.Infrastructure.Extensions;
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
            return DbSet
                .AsTracking(readOnly)
                .Include(usuario => usuario.Tokens.OrderByDescending(token => token.ExpiraEm))
                .FirstOrDefaultAsync(usuario => usuario.Id == id);
        }

        public async Task<Usuario> ObterPorEmailAsync(Email email)
        {
            return await DbSet
                .Include(usuario => usuario.Tokens.OrderByDescending(token => token.ExpiraEm))
                .FirstOrDefaultAsync(usuario => usuario.Email.Address == email.Address);
        }

        public async Task<Usuario> ObterPorTokenAtualizacaoAsync(string tokenAtualizacao)
        {
            return await DbSet
                .Include(usuario => usuario.Tokens.Where(token => token.Atualizacao == tokenAtualizacao))
                .FirstOrDefaultAsync(usuario => usuario.Tokens.Any(token => token.Atualizacao == tokenAtualizacao));
        }

        public async Task<bool> VerificarSeEmailExisteAsync(Email email)
            => await DbSet.AsNoTracking().AnyAsync(usuario => usuario.Email.Address == email.Address);
    }
}