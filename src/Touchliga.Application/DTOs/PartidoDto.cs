namespace Touchliga.Application.DTOs;

public sealed class PartidoDto
{
    public long Id { get; set; }

    public long JornadaId { get; set; }

    public long EquipoLocalId { get; set; }

    public long EquipoVisitanteId { get; set; }

    public DateTime FechaHora { get; set; }

    public long? CanchaId { get; set; }

    public string? CanchaNombre { get; set; }

    public int? GolesLocal { get; set; }

    public int? GolesVisitante { get; set; }

    public bool TieneResultado { get; set; }

    public bool EsDesempate { get; set; }
}
