using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SGP.Domain.Entities;
using SGP.Domain.Repositories;
using SGP.Domain.ValueObjects;
using SGP.Infrastructure.Data.Context;
using SGP.Infrastructure.Data.Repositories.Common;

namespace SGP.Infrastructure.Data.Repositories;

public class UsuarioRepository(SgpContext context) : EfRepository<Usuario>(context), IUsuarioRepository
{
    public override async Task<Usuario> GetByIdAsync(Guid id, bool readOnly = false) =>
        await (readOnly ? DbSet.AsNoTracking() : DbSet.AsQueryable())
            .Include(u => u.Tokens.OrderByDescending(t => t.ExpiraEm))
            .FirstOrDefaultAsync(u => u.Id == id);

    public async Task<Usuario> ObterPorEmailAsync(Email email) =>
        await DbSet
            .Include(u => u.Tokens.OrderByDescending(t => t.ExpiraEm))
            .FirstOrDefaultAsync(u => u.Email.Address == email.Address);

    public async Task<Usuario> ObterPorTokenAtualizacaoAsync(string tokenAtualizacao) =>
         await DbSet
            .Include(u => u.Tokens.Where(t => t.Atualizacao == tokenAtualizacao))
            .FirstOrDefaultAsync(u => u.Tokens.Any(t => t.Atualizacao == tokenAtualizacao));

    public async Task<bool> VerificarSeEmailExisteAsync(Email email) =>
        await DbSet
            .AsNoTracking()
            .AnyAsync(u => u.Email.Address == email.Address);
}