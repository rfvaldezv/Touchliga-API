namespace Touchliga.Application.DTOs;

/// <summary>Mi estatus de pago de una temporada — ahora puede ser
/// más de un pago (completo, o la mitad y luego el resto).</summary>
public sealed class ResumenPagoDto
{
    public decimal Cuota { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public bool PagoCompleto { get; set; }
    public List<PagoDto> Pagos { get; set; } = [];
}
