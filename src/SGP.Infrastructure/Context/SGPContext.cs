using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;
using System.Reflection;

namespace SGP.Infrastructure.Context
{
    public class SGPContext : DbContext
    {
        public SGPContext(DbContextOptions<SGPContext> options) : base(options)
        {
        }

        public override ChangeTracker ChangeTracker
        {
            get
            {
                base.ChangeTracker.LazyLoadingEnabled = false;
                return base.ChangeTracker;
            }
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Collation: define o conjunto de regras que o servidor irá utilizar para ordenação e comparação entre textos.
            // Configurado para ignorar o "Case Insensitive (CI)" e os acentos "Accent Insensitive (AI)".
            modelBuilder.UseCollation("Latin1_General_CI_AI");
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.RemoveCascadeDeleteConvention();
        }
    }
}