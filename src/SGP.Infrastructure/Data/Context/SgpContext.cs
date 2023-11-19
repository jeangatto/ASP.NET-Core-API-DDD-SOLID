using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;
using SGP.Shared.AppSettings;

namespace SGP.Infrastructure.Data.Context;

public sealed class SgpContext(DbContextOptions<SgpContext> dbOptions) : DbContext(dbOptions)
{
    private readonly string _collation;

    public SgpContext(IOptions<ConnectionStrings> options, DbContextOptions<SgpContext> dbOptions)
        : this(dbOptions)
    {
        _collation = options.Value.Collation;
    }

    public override ChangeTracker ChangeTracker
    {
        get
        {
            base.ChangeTracker.LazyLoadingEnabled = false;
            base.ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            base.ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
            return base.ChangeTracker;
        }
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