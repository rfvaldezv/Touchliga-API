using MediatR;

namespace Touchliga.Application.Commands.Pago.Registrar;

public sealed record RegistrarPagoCommand(
    long UsuarioId,
    long TemporadaId,
    decimal Monto,
    string MetodoPago,
    DateTime FechaPago,
    string? Referencia
)
    : IRequest<long>;
