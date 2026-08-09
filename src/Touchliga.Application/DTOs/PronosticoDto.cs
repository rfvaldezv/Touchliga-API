namespace Touchliga.Application.DTOs;

public sealed class PronosticoDto
{
    public long Id { get; set; }

    public long PartidoId { get; set; }

    public long UsuarioId { get; set; }

    public long EquipoGanadorId { get; set; }

    public int? Puntos { get; set; }

    public int? PuntosTotalesPredichos { get; set; }

    public int? DiferenciaPuntosPredicha { get; set; }

    public int PuntosBono { get; set; }
}
