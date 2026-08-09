namespace Touchliga.Application.DTOs;

public sealed class TemporadaDto
{
    public long Id { get; set; }

    public long LigaId { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    public decimal Cuota { get; set; }

    public bool Activo { get; set; }
}
