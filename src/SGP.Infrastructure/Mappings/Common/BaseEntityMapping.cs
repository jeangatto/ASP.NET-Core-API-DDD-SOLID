using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Shared.Entities;

namespace SGP.Infrastructure.Mappings.Common
{
    public class BaseEntityMapping<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Definindo a coluna "ID" como Chave Primária (PK).
            builder.HasKey(e => e.Id);

            // Definindo a coluna "ID" como NOT NULL.
            builder.Property(e => e.Id).IsRequired();
        }
    }
}