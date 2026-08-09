namespace Touchliga.Application.DTOs;

public sealed class DesgloseMetodoPagoDto
{
    public string MetodoPago { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Monto { get; set; }
}

public sealed class TransaccionDto
{
    public long Id { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }
    public string? Referencia { get; set; }
}

public sealed class ResumenFinancieroDto
{
    public decimal Cuota { get; set; }
    public int TotalParticipantes { get; set; }
    public int ParticipantesCubiertos { get; set; }
    public decimal TotalEsperado { get; set; }
    public decimal TotalRecaudado { get; set; }
    public decimal TotalPendiente { get; set; }
    public List<DesgloseMetodoPagoDto> DesglosePorMetodo { get; set; } = [];
    public List<TransaccionDto> UltimasTransacciones { get; set; } = [];
}
