using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Context
{
    public class SgpContext : DbContext
    {
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

        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Regiao> Regioes { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Collation: define o conjunto de regras que o servidor irá utilizar para ordenação e comparação entre textos.
            // NOTE: Configurado para ignorar o "Case Insensitive (CI)" e os acentos "Accent Insensitive (AI)".
            modelBuilder.UseCollation("Latin1_General_CI_AI");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.RemoveCascadeDeleteConvention();
        }
    }
}