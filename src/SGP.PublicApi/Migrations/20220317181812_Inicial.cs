using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGP.PublicApi.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regioes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Regioes", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    HashSenha = table.Column<string>(type: "varchar(60)", unicode: false, maxLength: 60, nullable: false),
                    UltimoAcessoEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BloqueioExpiraEm = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroFalhasAoAcessar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => table.PrimaryKey("PK_Usuarios", x => x.Id));

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegiaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(75)", unicode: false, maxLength: 75, nullable: false),
                    Uf = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estados_Regioes_RegiaoId",
                        column: x => x.RegiaoId,
                        principalTable: "Regioes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Acesso = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false, comment: "AcessToken"),
                    Atualizacao = table.Column<string>(type: "varchar(2048)", unicode: false, maxLength: 2048, nullable: false, comment: "RefreshToken"),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiraEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevogadoEm = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cidades",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EstadoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(70)", unicode: false, maxLength: 70, nullable: false),
                    Ibge = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cidades_Estados_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "Estados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades",
                column: "EstadoId")
                .Annotation("SqlServer:Include", new[] { "Nome", "Ibge" });

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_Ibge",
                table: "Cidades",
                column: "Ibge",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estados_RegiaoId",
                table: "Estados",
                column: "RegiaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Estados_Uf",
                table: "Estados",
                column: "Uf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regioes_Nome",
                table: "Regioes",
                column: "Nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UsuarioId",
                table: "Tokens",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cidades");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Regioes");
        }
    }
}