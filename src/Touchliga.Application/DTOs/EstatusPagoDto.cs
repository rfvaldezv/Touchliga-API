namespace Touchliga.Application.DTOs;

/// <summary>
/// Un participante y cuánto lleva pagado de la cuota de la
/// temporada — para la pantalla de administración de pagos. Puede
/// estar parcialmente pagado (pagó la mitad, falta el resto).
/// </summary>
public sealed class EstatusPagoDto
{
    public long UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public decimal Cuota { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public bool PagoCompleto { get; set; }
}
