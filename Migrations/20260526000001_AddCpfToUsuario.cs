using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoOS.Migrations
{
    /// <inheritdoc />
    public partial class AddCpfToUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CPF",
                table: "Usuarios",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_CPF",
                table: "Usuarios",
                column: "CPF",
                unique: true,
                filter: "\"CPF\" IS NOT NULL AND \"CPF\" <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_CPF",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CPF",
                table: "Usuarios");
        }
    }
}
