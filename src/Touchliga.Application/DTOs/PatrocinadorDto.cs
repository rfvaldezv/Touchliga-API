namespace Touchliga.Application.DTOs;

public sealed class PatrocinadorDto
{
    public long Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ImagenUrl { get; set; } = string.Empty;
    public string? EnlaceUrl { get; set; }
    public int Orden { get; set; }
    public bool Activo { get; set; }
}
