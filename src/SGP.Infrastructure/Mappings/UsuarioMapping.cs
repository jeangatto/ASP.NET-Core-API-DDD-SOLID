using Microsoft.EntityFrameworkCore;
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

            builder.ToTable(nameof(Usuario));

            builder.Property(u => u.Apelido)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(30);

            builder.Property(u => u.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(100);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Senha)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(60);

            builder.Property(u => u.DataUltimoAcesso)
                .IsRequired(false);

            builder.Property(u => u.DataBloqueio)
                .IsRequired(false);

            builder.Property(u => u.AcessosComSucesso)
                .IsRequired();

            builder.Property(u => u.AcessosComFalha)
                .IsRequired();

            builder.Ignore(u => u.ContaBloqueada);
        }
    }
}