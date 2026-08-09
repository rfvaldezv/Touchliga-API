using Touchliga.Domain.Entities;

namespace Touchliga.Domain.Interfaces;

public interface IPagoRepository
{
    Task AgregarAsync(Pago pago, CancellationToken cancellationToken = default);

    Task<Pago?> ObtenerPorIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>Todos los pagos de ese usuario en esa temporada — puede
    /// haber más de uno (ej. pagó la mitad, luego el resto).</summary>
    Task<IReadOnlyList<Pago>> ObtenerListaPorUsuarioYTemporadaAsync(
        long usuarioId,
        long temporadaId,
        CancellationToken cancellationToken = default);

    /// <summary>Para revisar si un pago de Stripe ya se registró antes
    /// (Stripe reintenta el mismo webhook seguido).</summary>
    Task<Pago?> ObtenerPorReferenciaAsync(string referencia, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Pago>> ObtenerPorTemporadaAsync(
        long temporadaId,
        CancellationToken cancellationToken = default);

    /// <summary>Todos los pagos de un usuario, en cualquier temporada
    /// — para la cuenta corriente completa del participante.</summary>
    Task<IReadOnlyList<Pago>> ObtenerPorUsuarioAsync(
        long usuarioId,
        CancellationToken cancellationToken = default);

    void Eliminar(Pago pago);
}
