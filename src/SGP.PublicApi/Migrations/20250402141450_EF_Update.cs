using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGP.PublicApi.Migrations
{
    /// <inheritdoc />
    public partial class EF_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades");

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades",
                column: "EstadoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades");

            migrationBuilder.CreateIndex(
                name: "IX_Cidades_EstadoId",
                table: "Cidades",
                column: "EstadoId")
                .Annotation("SqlServer:Include", new[] { "Nome", "Ibge" });
        }
    }
}
