using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP2ProgramacionWeb.Frontend.Migrations
{
    /// <inheritdoc />
    public partial class AgregarColumnaEliminadoProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "Productos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "Productos");
        }
    }
}
