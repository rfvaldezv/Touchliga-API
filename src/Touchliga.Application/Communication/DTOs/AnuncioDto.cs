namespace Touchliga.Application.Communication.DTOs;

public sealed class AnuncioDto
{
    public long Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Contenido { get; set; } = string.Empty;
    public string? ImagenUrl { get; set; }
    public string AutorNombre { get; set; } = string.Empty;
    public DateTime FechaPublicacion { get; set; }

    /// <summary>Conteo de reacciones por emoji, ej. {"👍": 5, "🔥": 2}.</summary>
    public Dictionary<string, int> Reacciones { get; set; } = new();

    /// <summary>El emoji con el que YO reaccioné, si acaso.</summary>
    public string? MiReaccion { get; set; }
}
