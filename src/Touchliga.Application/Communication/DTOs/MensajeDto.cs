namespace Touchliga.Application.Communication.DTOs;

public sealed class MensajeDto
{
    public long Id { get; set; }
    public long RemitenteId { get; set; }
    public long DestinatarioId { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }
    public DateTime FechaEnvio { get; set; }
    public bool Leido { get; set; }
    public bool EsMio { get; set; }
}
