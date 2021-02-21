using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Mappings.Common;

namespace SGP.Infrastructure.Mappings
{
    public class ClienteMapping : BaseEntityMapping<Cliente>
    {
        public override void Configure(EntityTypeBuilder<Cliente> builder)
        {
            base.Configure(builder);

            builder.ToTable(nameof(Cliente));

            builder.Property(c => c.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.OwnsOne(c => c.CPF, owned =>
            {
                owned.Property(o => o.Numero)
                    .IsRequired()
                    .HasColumnName(nameof(Cliente.CPF))
                    .IsUnicode(false)
                    .HasMaxLength(11);

                owned.HasIndex(o => o.Numero)
                    .IsUnique();
            });

            builder.Navigation(c => c.CPF)
                .IsRequired();

            builder.Property(c => c.Sexo)
                .HasConversion<short>()
                .IsRequired();

            builder.OwnsOne(c => c.DataNascimento)
                .Property(c => c.Data)
                .IsRequired()
                .HasColumnName(nameof(Cliente.DataNascimento));

            builder.Navigation(c => c.DataNascimento)
                .IsRequired();

            builder.OwnsOne(c => c.DataCadastro)
                .Property(c => c.Data)
                .IsRequired()
                .HasColumnName(nameof(Cliente.DataCadastro));

            builder.Navigation(c => c.DataCadastro)
                .IsRequired();
        }
    }
}
