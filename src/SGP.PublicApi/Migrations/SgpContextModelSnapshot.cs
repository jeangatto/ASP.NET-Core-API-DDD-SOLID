// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SGP.Infrastructure.Context;

#nullable disable

namespace SGP.PublicApi.Migrations
{
    [DbContext(typeof(SgpContext))]
    partial class SgpContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Latin1_General_CI_AI")
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SGP.Domain.Entities.Cidade", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EstadoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Ibge")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(70)
                        .IsUnicode(false)
                        .HasColumnType("varchar(70)");

                    b.HasKey("Id");

                    b.HasIndex("EstadoId");

                    SqlServerIndexBuilderExtensions.IncludeProperties(b.HasIndex("EstadoId"), new[] { "Nome", "Ibge" });

                    b.HasIndex("Ibge")
                        .IsUnique();

                    b.ToTable("Cidades");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Estado", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(75)
                        .IsUnicode(false)
                        .HasColumnType("varchar(75)");

                    b.Property<Guid>("RegiaoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Uf")
                        .IsRequired()
                        .HasMaxLength(2)
                        .IsUnicode(false)
                        .HasColumnType("char(2)")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.HasIndex("RegiaoId");

                    b.HasIndex("Uf")
                        .IsUnique();

                    b.ToTable("Estados");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Regiao", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(15)
                        .IsUnicode(false)
                        .HasColumnType("varchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("Nome")
                        .IsUnique();

                    b.ToTable("Regioes");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Acesso")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .IsUnicode(false)
                        .HasColumnType("varchar(2048)")
                        .HasComment("AcessToken");

                    b.Property<string>("Atualizacao")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .IsUnicode(false)
                        .HasColumnType("varchar(2048)")
                        .HasComment("RefreshToken");

                    b.Property<DateTime>("CriadoEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiraEm")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RevogadoEm")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UsuarioId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Usuario", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("BloqueioExpiraEm")
                        .HasColumnType("datetime2");

                    b.Property<string>("HashSenha")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("NumeroFalhasAoAcessar")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UltimoAcessoEm")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Cidade", b =>
                {
                    b.HasOne("SGP.Domain.Entities.Estado", "Estado")
                        .WithMany("Cidades")
                        .HasForeignKey("EstadoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Estado");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Estado", b =>
                {
                    b.HasOne("SGP.Domain.Entities.Regiao", "Regiao")
                        .WithMany("Estados")
                        .HasForeignKey("RegiaoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Regiao");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Token", b =>
                {
                    b.HasOne("SGP.Domain.Entities.Usuario", "Usuario")
                        .WithMany("Tokens")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Usuario", b =>
                {
                    b.OwnsOne("SGP.Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UsuarioId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Address")
                                .IsRequired()
                                .HasMaxLength(100)
                                .IsUnicode(false)
                                .HasColumnType("varchar(100)")
                                .HasColumnName("Email");

                            b1.HasKey("UsuarioId");

                            b1.HasIndex("Address")
                                .IsUnique()
                                .HasFilter("[Email] IS NOT NULL");

                            b1.ToTable("Usuarios");

                            b1.WithOwner()
                                .HasForeignKey("UsuarioId");
                        });

                    b.Navigation("Email");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Estado", b =>
                {
                    b.Navigation("Cidades");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Regiao", b =>
                {
                    b.Navigation("Estados");
                });

            modelBuilder.Entity("SGP.Domain.Entities.Usuario", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
