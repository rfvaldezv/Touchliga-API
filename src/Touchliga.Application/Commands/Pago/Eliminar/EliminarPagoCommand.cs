using MediatR;

namespace Touchliga.Application.Commands.Pago.Eliminar;

public sealed record EliminarPagoCommand(long Id) : IRequest<Unit>;
