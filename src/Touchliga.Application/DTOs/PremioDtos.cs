namespace Touchliga.Application.DTOs;

public sealed class ConfiguracionPremioDto
{
    public long Id { get; set; }
    public int Posicion { get; set; }
    public string TipoPremio { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
}

public sealed class GanadorParticipanteDto
{
    public long UsuarioId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Puntos { get; set; }
    public decimal MontoSugerido { get; set; }

    /// <summary>"Pendiente" (todavía nadie decidió), "Aprobado" o "Denegado".</summary>
    public string Estado { get; set; } = "Pendiente";

    /// <summary>Si el responsable ajustó el monto al aprobar; si es
    /// null, se paga el sugerido tal cual.</summary>
    public decimal? MontoAjustado { get; set; }

    public string? Motivo { get; set; }
}

public sealed class GanadorPremioDto
{
    public int PosicionDesde { get; set; }
    public int PosicionHasta { get; set; }
    public List<GanadorParticipanteDto> Participantes { get; set; } = [];
    public string TipoPremio { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool HuboEmpate { get; set; }
}
