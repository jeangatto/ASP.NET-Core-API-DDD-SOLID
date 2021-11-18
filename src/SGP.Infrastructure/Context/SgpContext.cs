using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;
using SGP.Infrastructure.Mappings;

namespace SGP.Infrastructure.Context
{
    public class SgpContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public SgpContext(DbContextOptions<SgpContext> options, ILoggerFactory loggerFactory) : base(options)
            => _loggerFactory = loggerFactory;

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
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseLoggerFactory(_loggerFactory);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Collation: define o conjunto de regras que o servidor irá utilizar para ordenação e comparação entre textos.
            // NOTE: Configurado para ignorar o "Case Insensitive (CI)" e os acentos "Accent Insensitive (AI)".
            modelBuilder.UseCollation("Latin1_General_CI_AI");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioMap).Assembly);
            modelBuilder.RemoveCascadeDeleteConvention();
        }
    }
}