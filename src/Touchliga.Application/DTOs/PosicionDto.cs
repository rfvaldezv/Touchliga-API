namespace Touchliga.Application.DTOs;

public sealed class PosicionDto
{
    public long UsuarioId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int Puntos { get; set; }

    public int Aciertos { get; set; }

    public int Pronosticos { get; set; }
}
