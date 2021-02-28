using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class CidadeMapping : IEntityTypeConfiguration<Cidade>
    {
        public void Configure(EntityTypeBuilder<Cidade> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(cidade => cidade.EstadoId)
                .IsRequired();

            builder.Property(cidade => cidade.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(70);

            builder.Property(cidade => cidade.Ibge)
                .IsRequired();

            builder.HasOne(cidade => cidade.Estado)
                .WithMany(estado => estado.Cidades)
                .HasForeignKey(cidade => cidade.EstadoId);
        }
    }
}
