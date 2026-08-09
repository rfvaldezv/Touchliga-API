using MediatR;

namespace Touchliga.Application.Commands.Partido.Update;

public sealed record UpdatePartidoCommand(
    long Id,
    long EquipoLocalId,
    long EquipoVisitanteId,
    DateTime FechaHora,
    long? CanchaId
)
    : IRequest<Unit>;
