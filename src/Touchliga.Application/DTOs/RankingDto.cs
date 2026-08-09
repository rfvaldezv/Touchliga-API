namespace Touchliga.Application.DTOs;

public sealed class RankingDto
{
    public long UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public List<PuntosPorJornadaDto> Jornadas { get; set; } = [];
    public int TotalPuntos { get; set; }
    public double PorcentajeProductividad { get; set; }
}

public sealed class PuntosPorJornadaDto
{
    public long JornadaId { get; set; }
    public int Numero { get; set; }
    public int Puntos { get; set; }
    public int Calificados { get; set; }
}
