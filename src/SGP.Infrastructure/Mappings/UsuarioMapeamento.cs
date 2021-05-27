using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class UsuarioMapeamento : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(usuario => usuario.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            // Mapeamento ValueObject
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

            builder.Property(usuario => usuario.HashSenha)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(60);

            builder.Property(usuario => usuario.UltimoAcessoEm)
                .IsRequired(false);

            builder.Property(usuario => usuario.BloqueioExpiraEm)
                .IsRequired(false);

            builder.Property(usuario => usuario.NumeroFalhasAcesso)
                .IsRequired();

            builder.Navigation(usuario => usuario.Tokens)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
