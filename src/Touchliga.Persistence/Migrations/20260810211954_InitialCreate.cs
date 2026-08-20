using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Touchliga.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "com");

            migrationBuilder.EnsureSchema(
                name: "cat");

            migrationBuilder.EnsureSchema(
                name: "seg");

            migrationBuilder.CreateTable(
                name: "Archivo",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreArchivo = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Datos = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    UsuarioSubioId = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Archivo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cancha",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cancha", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionSmtp",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Habilitado = table.Column<bool>(type: "bit", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FromEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionSmtp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Deporte",
                schema: "cat",
                columns: table => new
                {
                    DeporteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deporte", x => x.DeporteId);
                });

            migrationBuilder.CreateTable(
                name: "Equipo",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EscudoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Apodo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jugador",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ligas",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ligas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pais",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pais", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patrocinador",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagenUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EnlaceUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patrocinador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permiso",
                schema: "seg",
                columns: table => new
                {
                    PermisoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permiso", x => x.PermisoId);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                schema: "seg",
                columns: table => new
                {
                    RolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "Temporada",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LigaId = table.Column<long>(type: "bigint", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cuota = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Temporada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Temporada_Ligas_LigaId",
                        column: x => x.LigaId,
                        principalSchema: "cat",
                        principalTable: "Ligas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Estado",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaisId = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Estado_Pais_PaisId",
                        column: x => x.PaisId,
                        principalSchema: "cat",
                        principalTable: "Pais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfiguracionPremio",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemporadaId = table.Column<long>(type: "bigint", nullable: false),
                    Ambito = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Posicion = table.Column<int>(type: "int", nullable: false),
                    TipoPremio = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionPremio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionPremio_Temporada_TemporadaId",
                        column: x => x.TemporadaId,
                        principalSchema: "cat",
                        principalTable: "Temporada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jornada",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemporadaId = table.Column<long>(type: "bigint", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cerrada = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jornada", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jornada_Temporada_TemporadaId",
                        column: x => x.TemporadaId,
                        principalSchema: "cat",
                        principalTable: "Temporada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ciudad",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaisId = table.Column<long>(type: "bigint", nullable: false),
                    EstadoId = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciudad", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciudad_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalSchema: "cat",
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ciudad_Pais_PaisId",
                        column: x => x.PaisId,
                        principalSchema: "cat",
                        principalTable: "Pais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Partido",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JornadaId = table.Column<long>(type: "bigint", nullable: false),
                    EquipoLocalId = table.Column<long>(type: "bigint", nullable: false),
                    EquipoVisitanteId = table.Column<long>(type: "bigint", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CanchaId = table.Column<long>(type: "bigint", nullable: true),
                    GolesLocal = table.Column<int>(type: "int", nullable: true),
                    GolesVisitante = table.Column<int>(type: "int", nullable: true),
                    EsDesempate = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partido", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Partido_Cancha_CanchaId",
                        column: x => x.CanchaId,
                        principalSchema: "cat",
                        principalTable: "Cancha",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_EquipoLocalId",
                        column: x => x.EquipoLocalId,
                        principalSchema: "cat",
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partido_Equipo_EquipoVisitanteId",
                        column: x => x.EquipoVisitanteId,
                        principalSchema: "cat",
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Partido_Jornada_JornadaId",
                        column: x => x.JornadaId,
                        principalSchema: "cat",
                        principalTable: "Jornada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "seg",
                columns: table => new
                {
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EmailConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    InvitadoPorId = table.Column<long>(type: "bigint", nullable: true),
                    CiudadId = table.Column<long>(type: "bigint", nullable: true),
                    PaisId = table.Column<long>(type: "bigint", nullable: true),
                    EstadoId = table.Column<long>(type: "bigint", nullable: true),
                    Sexo = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Estatus = table.Column<int>(type: "int", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EquipoFavoritoId = table.Column<long>(type: "bigint", nullable: true),
                    Nickname = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FotoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuario_Ciudad_CiudadId",
                        column: x => x.CiudadId,
                        principalSchema: "cat",
                        principalTable: "Ciudad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Equipo_EquipoFavoritoId",
                        column: x => x.EquipoFavoritoId,
                        principalSchema: "cat",
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Estado_EstadoId",
                        column: x => x.EstadoId,
                        principalSchema: "cat",
                        principalTable: "Estado",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Pais_PaisId",
                        column: x => x.PaisId,
                        principalSchema: "cat",
                        principalTable: "Pais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Usuario_Usuario_InvitadoPorId",
                        column: x => x.InvitadoPorId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Anuncio",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UsuarioAutorId = table.Column<long>(type: "bigint", nullable: false),
                    FechaPublicacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anuncio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Anuncio_Usuario_UsuarioAutorId",
                        column: x => x.UsuarioAutorId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Mensaje",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RemitenteId = table.Column<long>(type: "bigint", nullable: false),
                    DestinatarioId = table.Column<long>(type: "bigint", nullable: false),
                    Contenido = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ImagenUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Leido = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensaje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mensaje_Usuario_DestinatarioId",
                        column: x => x.DestinatarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensaje_Usuario_RemitenteId",
                        column: x => x.RemitenteId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    TemporadaId = table.Column<long>(type: "bigint", nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    MetodoPago = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RegistradoPorId = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pago_Temporada_TemporadaId",
                        column: x => x.TemporadaId,
                        principalSchema: "cat",
                        principalTable: "Temporada",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pago_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PremioOtorgado",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ambito = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReferenciaId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    MontoAjustado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Motivo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DecididoPorId = table.Column<long>(type: "bigint", nullable: false),
                    FechaDecision = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PremioOtorgado", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PremioOtorgado_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pronostico",
                schema: "cat",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartidoId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    EquipoGanadorId = table.Column<long>(type: "bigint", nullable: false),
                    Puntos = table.Column<int>(type: "int", nullable: true),
                    PuntosTotalesPredichos = table.Column<int>(type: "int", nullable: true),
                    DiferenciaPuntosPredicha = table.Column<int>(type: "int", nullable: true),
                    PuntosBono = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pronostico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pronostico_Equipo_EquipoGanadorId",
                        column: x => x.EquipoGanadorId,
                        principalSchema: "cat",
                        principalTable: "Equipo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pronostico_Partido_PartidoId",
                        column: x => x.PartidoId,
                        principalSchema: "cat",
                        principalTable: "Partido",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pronostico_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PushToken",
                schema: "seg",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Plataforma = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PushToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PushToken_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "seg",
                columns: table => new
                {
                    RefreshTokenId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Expira = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revocado = table.Column<bool>(type: "bit", nullable: false),
                    FechaRevocacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MotivoRevocacion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.RefreshTokenId);
                    table.ForeignKey(
                        name: "FK_RefreshToken_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sesion",
                schema: "seg",
                columns: table => new
                {
                    SesionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DireccionIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Dispositivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SistemaOperativo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Navegador = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sesion", x => x.SesionId);
                    table.ForeignKey(
                        name: "FK_Sesion_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioRol",
                schema: "seg",
                columns: table => new
                {
                    UsuarioRolId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    RolId = table.Column<long>(type: "bigint", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioRol", x => x.UsuarioRolId);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Rol_RolId",
                        column: x => x.RolId,
                        principalSchema: "seg",
                        principalTable: "Rol",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioRol_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReaccionAnuncio",
                schema: "com",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnuncioId = table.Column<long>(type: "bigint", nullable: false),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    Emoji = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FechaReaccion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAltaId = table.Column<long>(type: "bigint", nullable: true),
                    FechaModificacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioModificacionId = table.Column<long>(type: "bigint", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReaccionAnuncio", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReaccionAnuncio_Anuncio_AnuncioId",
                        column: x => x.AnuncioId,
                        principalSchema: "com",
                        principalTable: "Anuncio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReaccionAnuncio_Usuario_UsuarioId",
                        column: x => x.UsuarioId,
                        principalSchema: "seg",
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anuncio_UsuarioAutorId",
                schema: "com",
                table: "Anuncio",
                column: "UsuarioAutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudad_EstadoId",
                schema: "cat",
                table: "Ciudad",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ciudad_PaisId",
                schema: "cat",
                table: "Ciudad",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionPremio_TemporadaId_Ambito_Posicion",
                schema: "com",
                table: "ConfiguracionPremio",
                columns: new[] { "TemporadaId", "Ambito", "Posicion" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deporte_Codigo",
                schema: "cat",
                table: "Deporte",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estado_PaisId",
                schema: "cat",
                table: "Estado",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Jornada_TemporadaId",
                schema: "cat",
                table: "Jornada",
                column: "TemporadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_DestinatarioId",
                schema: "com",
                table: "Mensaje",
                column: "DestinatarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Mensaje_RemitenteId_DestinatarioId",
                schema: "com",
                table: "Mensaje",
                columns: new[] { "RemitenteId", "DestinatarioId" });

            migrationBuilder.CreateIndex(
                name: "IX_Pago_TemporadaId",
                schema: "com",
                table: "Pago",
                column: "TemporadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_UsuarioId_TemporadaId",
                schema: "com",
                table: "Pago",
                columns: new[] { "UsuarioId", "TemporadaId" });

            migrationBuilder.CreateIndex(
                name: "IX_Partido_CanchaId",
                schema: "cat",
                table: "Partido",
                column: "CanchaId");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_EquipoLocalId",
                schema: "cat",
                table: "Partido",
                column: "EquipoLocalId");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_EquipoVisitanteId",
                schema: "cat",
                table: "Partido",
                column: "EquipoVisitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Partido_JornadaId",
                schema: "cat",
                table: "Partido",
                column: "JornadaId");

            migrationBuilder.CreateIndex(
                name: "IX_Permiso_Codigo",
                schema: "seg",
                table: "Permiso",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PremioOtorgado_Ambito_ReferenciaId_UsuarioId",
                schema: "com",
                table: "PremioOtorgado",
                columns: new[] { "Ambito", "ReferenciaId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PremioOtorgado_UsuarioId",
                schema: "com",
                table: "PremioOtorgado",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Pronostico_EquipoGanadorId",
                schema: "cat",
                table: "Pronostico",
                column: "EquipoGanadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Pronostico_PartidoId_UsuarioId",
                schema: "cat",
                table: "Pronostico",
                columns: new[] { "PartidoId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pronostico_UsuarioId",
                schema: "cat",
                table: "Pronostico",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_PushToken_Token",
                schema: "seg",
                table: "PushToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PushToken_UsuarioId",
                schema: "seg",
                table: "PushToken",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ReaccionAnuncio_AnuncioId_UsuarioId",
                schema: "com",
                table: "ReaccionAnuncio",
                columns: new[] { "AnuncioId", "UsuarioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReaccionAnuncio_UsuarioId",
                schema: "com",
                table: "ReaccionAnuncio",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                schema: "seg",
                table: "RefreshToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UsuarioId",
                schema: "seg",
                table: "RefreshToken",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Sesion_UsuarioId",
                schema: "seg",
                table: "Sesion",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Temporada_LigaId",
                schema: "cat",
                table: "Temporada",
                column: "LigaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_CiudadId",
                schema: "seg",
                table: "Usuario",
                column: "CiudadId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Correo",
                schema: "seg",
                table: "Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EquipoFavoritoId",
                schema: "seg",
                table: "Usuario",
                column: "EquipoFavoritoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_EstadoId",
                schema: "seg",
                table: "Usuario",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_InvitadoPorId",
                schema: "seg",
                table: "Usuario",
                column: "InvitadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PaisId",
                schema: "seg",
                table: "Usuario",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_RolId",
                schema: "seg",
                table: "UsuarioRol",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioRol_UsuarioId_RolId",
                schema: "seg",
                table: "UsuarioRol",
                columns: new[] { "UsuarioId", "RolId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Archivo",
                schema: "com");

            migrationBuilder.DropTable(
                name: "Categoria",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "ConfiguracionPremio",
                schema: "com");

            migrationBuilder.DropTable(
                name: "ConfiguracionSmtp",
                schema: "com");

            migrationBuilder.DropTable(
                name: "Deporte",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Jugador",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Mensaje",
                schema: "com");

            migrationBuilder.DropTable(
                name: "Pago",
                schema: "com");

            migrationBuilder.DropTable(
                name: "Patrocinador",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Permiso",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "PremioOtorgado",
                schema: "com");

            migrationBuilder.DropTable(
                name: "Pronostico",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "PushToken",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "ReaccionAnuncio",
                schema: "com");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "Sesion",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "UsuarioRol",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "Partido",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Anuncio",
                schema: "com");

            migrationBuilder.DropTable(
                name: "Rol",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "Cancha",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Jornada",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Usuario",
                schema: "seg");

            migrationBuilder.DropTable(
                name: "Temporada",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Ciudad",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Equipo",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Ligas",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Estado",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "Pais",
                schema: "cat");
        }
    }
}
