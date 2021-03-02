using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class EstadoMapping : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(estado => estado.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(20);

            builder.Property(estado => estado.Sigla)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(2)
                .IsFixedLength();

            builder.HasIndex(estado => estado.Sigla)
                .IsUnique();

            builder.Property(estado => estado.Ibge)
                .IsRequired();

            builder.Property(estado => estado.Regiao)
                .IsRequired()
                .HasConversion<string>()
                .IsUnicode(false)
                .HasMaxLength(12);

            builder.HasOne(estado => estado.Pais)
                .WithMany(pais => pais.Estados)
                .HasForeignKey(estado => estado.PaisId);
        }
    }
}