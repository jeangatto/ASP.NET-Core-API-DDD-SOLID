using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class PaisMapping : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(pais => pais.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(60);

            builder.HasIndex(pais => pais.Sigla)
                .IsUnique();

            builder.Property(pais => pais.Sigla)
                .IsRequired()
                .IsUnicode(false)
                .IsFixedLength()
                .HasMaxLength(2);

            builder.Property(pais => pais.Bacen)
                .IsRequired();

            builder.HasIndex(pais => pais.Bacen)
                .IsUnique();
        }
    }
}
