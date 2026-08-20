using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Touchliga.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AgregarParejaYCredencialAlterna : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NombreEquipo",
                schema: "seg",
                table: "Usuario",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParejaId",
                schema: "seg",
                table: "Usuario",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CredencialAlterna",
                schema: "seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CredencialAlterna", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CredencialAlterna_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_ParejaId",
                schema: "seg",
                table: "Usuario",
                column: "ParejaId");

            migrationBuilder.CreateIndex(
                name: "IX_CredencialAlterna_Correo",
                schema: "seg",
                table: "CredencialAlterna",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CredencialAlterna_UsuarioId",
                schema: "seg",
                table: "CredencialAlterna",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Usuario_ParejaId",
                schema: "seg",
                table: "Usuario",
                column: "ParejaId",
                principalSchema: "seg",
                principalTable: "Usuario",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Usuario_ParejaId",
                schema: "seg",
                table: "Usuario");

            migrationBuilder.DropTable(
                name: "CredencialAlterna",
                schema: "seg");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_ParejaId",
                schema: "seg",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "NombreEquipo",
                schema: "seg",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "ParejaId",
                schema: "seg",
                table: "Usuario");
        }
    }
}
