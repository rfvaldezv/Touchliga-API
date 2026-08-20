using Touchliga.Domain.Common;
using Touchliga.Domain.Exceptions;

namespace Touchliga.Domain.Entities;

/// <summary>
/// Registro manual de un pago de cuota (fase 1: el admin marca a mano
/// quién pagó — efectivo, transferencia, etc. Fase 2, futura: cobro
/// real con procesador de pagos, sin cambiar este modelo).
/// </summary>
public sealed class Pago : AggregateRoot
{
    private Pago()
    {
    }

    public long UsuarioId { get; private set; }

    public long TemporadaId { get; private set; }

    public decimal Monto { get; private set; }

    public string MetodoPago { get; private set; } = string.Empty;

    public DateTime FechaPago { get; private set; }

    public string? Referencia { get; private set; }

    public long RegistradoPorId { get; private set; }

    public static Pago Registrar(
        long usuarioId,
        long temporadaId,
        decimal monto,
        string metodoPago,
        DateTime fechaPago,
        string? referencia,
        long registradoPorId)
    {
        if (monto <= 0)
            throw new DomainException("El monto debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(metodoPago))
            throw new DomainException("El método de pago es obligatorio.");

        return new Pago
        {
            UsuarioId = usuarioId,
            TemporadaId = temporadaId,
            Monto = monto,
            MetodoPago = metodoPago.Trim(),
            FechaPago = fechaPago,
            Referencia = string.IsNullOrWhiteSpace(referencia) ? null : referencia.Trim(),
            RegistradoPorId = registradoPorId,
            UsuarioAltaId = registradoPorId,
            FechaAlta = DateTime.UtcNow,
            Activo = true
        };
    }

    /// <summary>Corrige un pago ya registrado -- pensado sobre todo
    /// para arreglar datos que llegaron mal desde la migración del
    /// sistema viejo.</summary>
    public void Editar(
        decimal monto,
        string metodoPago,
        DateTime fechaPago,
        string? referencia,
        long usuarioId)
    {
        if (monto <= 0)
            throw new DomainException("El monto debe ser mayor a cero.");

        if (string.IsNullOrWhiteSpace(metodoPago))
            throw new DomainException("El método de pago es obligatorio.");

        Monto = monto;
        MetodoPago = metodoPago.Trim();
        FechaPago = fechaPago;
        Referencia = string.IsNullOrWhiteSpace(referencia) ? null : referencia.Trim();
        MarcarModificado(usuarioId);
    }
}
