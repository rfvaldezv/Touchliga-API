using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Touchliga.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarEsCuentaVinculada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EsCuentaVinculada",
                schema: "seg",
                table: "Usuario",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EsCuentaVinculada",
                schema: "seg",
                table: "Usuario");
        }
    }
}
