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

        /// <summary>
        /// Collation: define o conjunto de regras que o servidor irá utilizar para ordenação e comparação entre textos.
        /// Latin1_General_CI_AI: Configurado para ignorar o "Case Insensitive (CI)" e os acentos "Accent Insensitive (AI)".
        /// </summary>
        private const string Collation = "Latin1_General_CI_AI";

        public SgpContext(DbContextOptions<SgpContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            _loggerFactory = loggerFactory;
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
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation(Collation);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UsuarioMap).Assembly);
            modelBuilder.RemoveCascadeDeleteConvention();
        }
    }
}