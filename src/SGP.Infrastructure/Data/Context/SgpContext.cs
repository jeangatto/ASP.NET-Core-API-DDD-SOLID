using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;
using SGP.Shared.AppSettings;

namespace SGP.Infrastructure.Data.Context;

public sealed class SgpContext : DbContext
{
    private readonly string _collation;

    public SgpContext(DbContextOptions<SgpContext> dbOptions) : base(dbOptions)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public SgpContext(IOptions<ConnectionOptions> options, DbContextOptions<SgpContext> dbOptions) : this(dbOptions)
    {
        _collation = options.Value.Collation;
    }

    public DbSet<Cidade> Cidades => Set<Cidade>();
    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Regiao> Regioes => Set<Regiao>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!string.IsNullOrWhiteSpace(_collation))
            modelBuilder.UseCollation(_collation);

        modelBuilder
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            .RemoveCascadeDeleteConvention();
    }
}