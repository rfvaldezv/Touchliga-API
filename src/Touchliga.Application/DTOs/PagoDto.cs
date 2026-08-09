namespace Touchliga.Application.DTOs;

public sealed class PagoDto
{
    public long Id { get; set; }
    public long UsuarioId { get; set; }
    public long TemporadaId { get; set; }
    public decimal Monto { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; }
    public string? Referencia { get; set; }
}
