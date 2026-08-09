namespace Touchliga.Application.DTOs;

/// <summary>Cuenta corriente completa de un participante — todas
/// las temporadas con cuota, cuánto debe/pagó en cada una.</summary>
public sealed class CuentaCorrienteDto
{
    public long UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = string.Empty;
    public decimal TotalAdeudado { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal SaldoTotal { get; set; }
    public List<CuentaCorrienteTemporadaDto> Temporadas { get; set; } = [];
}

public sealed class CuentaCorrienteTemporadaDto
{
    public long TemporadaId { get; set; }
    public string TemporadaNombre { get; set; } = string.Empty;
    public decimal Cuota { get; set; }
    public decimal TotalPagado { get; set; }
    public decimal SaldoPendiente { get; set; }
    public bool PagoCompleto { get; set; }
    public List<PagoDto> Pagos { get; set; } = [];
}
