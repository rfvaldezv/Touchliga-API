using MediatR;

namespace Touchliga.Application.Commands.Pago.Editar;

public sealed record EditarPagoCommand(
    long Id,
    decimal Monto,
    string MetodoPago,
    DateTime FechaPago,
    string? Referencia
) : IRequest<Unit>;
