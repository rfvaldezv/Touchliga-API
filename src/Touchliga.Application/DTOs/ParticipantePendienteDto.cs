namespace Touchliga.Application.DTOs;

public sealed class ParticipantePendienteDto
{
    public long UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public int PartidosCapturados { get; set; }
    public int TotalPartidos { get; set; }
}
