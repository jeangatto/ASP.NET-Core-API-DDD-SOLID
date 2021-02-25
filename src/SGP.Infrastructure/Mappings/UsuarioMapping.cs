using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SGP.Domain.Entities;
using SGP.Infrastructure.Mappings.Common;

namespace SGP.Infrastructure.Mappings
{
    public class UsuarioMapping : BaseEntityMapping<Usuario>
    {
        public override void Configure(EntityTypeBuilder<Usuario> builder)
        {
            base.Configure(builder);

            builder.Property(usuario => usuario.Apelido)
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