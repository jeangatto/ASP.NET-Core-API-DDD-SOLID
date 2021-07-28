using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class CidadeMapeamento : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(cidade => cidade.EstadoId)
                .IsRequired();

            builder.Property(cidade => cidade.Nome)
                .IsRequired()
                .HasMaxLength(70)
                .IsUnicode(false);

            builder.Property(cidade => cidade.Ibge)
                .IsRequired();

            builder.HasIndex(cidade => cidade.Ibge)
                .IsUnique();

            builder.HasOne(cidade => cidade.Estado)
                .WithMany(estado => estado.Cidades)
                .HasForeignKey(cidade => cidade.EstadoId);
        }
    }
}