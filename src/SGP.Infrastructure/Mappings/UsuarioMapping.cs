using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities.UsuarioAggregate;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ConfigureSingularTableName();
            builder.ConfigureBaseEntity();

            builder.Property(usuario => usuario.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            // Mapeamento Value Objects
            builder.OwnsOne(usuario => usuario.Email, ownedNav =>
            {
                ownedNav.Property(email => email.Address)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(100)
                    .HasColumnName("Email");

                ownedNav.HasIndex(email => email.Address)
                    .IsUnique();
            });

            builder.Property(usuario => usuario.Senha)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(60);

            builder.Property(usuario => usuario.DataUltimoAcesso)
                .IsRequired(false);

            builder.Property(usuario => usuario.DataBloqueio)
                .IsRequired(false);

            builder.Property(usuario => usuario.AcessosComSucesso)
                .IsRequired();

            builder.Property(usuario => usuario.AcessosComFalha)
                .IsRequired();

            builder.Navigation(usuario => usuario.RefreshTokens)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}