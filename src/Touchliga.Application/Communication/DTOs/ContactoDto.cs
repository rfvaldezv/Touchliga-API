namespace Touchliga.Application.Communication.DTOs;

/// <summary>
/// Un contacto en la bandeja de entrada: el último mensaje
/// intercambiado con esa persona, y si hay pendientes por leer.
/// </summary>
public sealed class ContactoDto
{
    public long UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string UltimoMensaje { get; set; } = string.Empty;
    public DateTime FechaUltimoMensaje { get; set; }
    public bool TieneNoLeidos { get; set; }
    public List<string> Roles { get; set; } = new();
}
