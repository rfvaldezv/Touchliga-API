namespace Touchliga.Application.Users.DTOs;

public sealed class UsuarioAdminDto
{
    public long Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public string Estatus { get; set; } = string.Empty;
    public string Sexo { get; set; } = string.Empty;
    public DateTime? FechaNacimiento { get; set; }
    public string? Nickname { get; set; }
    public long? EquipoFavoritoId { get; set; }
    public string? EquipoFavoritoNombre { get; set; }
    public string? FotoUrl { get; set; }
    public List<string> Roles { get; set; } = new();

    public long? InvitadoPorId { get; set; }
    public string? InvitadoPorNombre { get; set; }

    public long? ParejaId { get; set; }
    public string? ParejaNombre { get; set; }
    public string? NombreEquipo { get; set; }

    public string? CorreoAlterna { get; set; }
    public bool EsCuentaVinculada { get; set; }

    public long? CiudadId { get; set; }
    public string? CiudadNombre { get; set; }

    public long? PaisId { get; set; }
    public string? PaisNombre { get; set; }

    public long? EstadoId { get; set; }
    public string? EstadoNombre { get; set; }
}
