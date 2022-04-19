using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Context;

public class SgpContext : DbContext
{
    /// <summary>
    /// Collation: define o conjunto de regras que o servidor irá utilizar para ordenação e comparação entre textos.
    /// Latin1_General_CI_AI: Configurado para ignorar o "Case Insensitive (CI)" e os acentos "Accent Insensitive (AI)".
    /// </summary>
    private const string DbCollation = "Latin1_General_CI_AI";

    public SgpContext(DbContextOptions<SgpContext> options) : base(options)
    {
    }

    public override ChangeTracker ChangeTracker
    {
        get
        {
            // Desabilitando o JOIN automático.
            base.ChangeTracker.LazyLoadingEnabled = false;
            return base.ChangeTracker;
        }
    }

    public DbSet<Cidade> Cidades => Set<Cidade>();
    public DbSet<Estado> Estados => Set<Estado>();
    public DbSet<Regiao> Regioes => Set<Regiao>();
    public DbSet<Token> Tokens => Set<Token>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder
            .UseCollation(DbCollation)
            .ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())
            .RemoveCascadeDeleteConvention();
}