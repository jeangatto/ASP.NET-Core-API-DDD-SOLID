using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Extensions;

namespace SGP.Infrastructure.Mappings
{
    public class UsuarioMapping : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ConfigureBaseEntity();

            builder.Property(usuario => usuario.Nome)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            builder.Property(usuario => usuario.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.HasIndex(usuario => usuario.Email)
                .IsUnique();

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

            builder.Ignore(usuario => usuario.ContaBloqueada);
        }
    }
}