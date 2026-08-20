namespace Touchliga.Application.DTOs;

public sealed class DetalleJornadaDto
{
    public long UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public List<DetallePartidoDto> Partidos { get; set; } = [];
    public int Total { get; set; }
}

public sealed class DetallePartidoDto
{
    public long PartidoId { get; set; }
    public string? EscudoLocalUrl { get; set; }
    public string? EscudoVisitanteUrl { get; set; }
    public string LocalNombre { get; set; } = string.Empty;
    public string VisitanteNombre { get; set; } = string.Empty;
    public long? EquipoGanadorReal { get; set; }
    public long? EquipoGanadorPronostico { get; set; }
    public bool EsDesempate { get; set; }
    public int? PuntosTotalesPredichos { get; set; }
    public int? DiferenciaPuntosPredicha { get; set; }
    public int? PuntosTotalesReal { get; set; }
    public int? DiferenciaPuntosReal { get; set; }
    /// <summary>null = todavía sin resultado capturado, o el usuario no pronosticó este partido.</summary>
    public int? Puntos { get; set; }
    public int PuntosBono { get; set; }
}
