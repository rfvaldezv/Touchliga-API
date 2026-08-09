using MediatR;

namespace Touchliga.Application.Commands.Partido.Create;

public sealed record CreatePartidoCommand(
    long JornadaId,
    long EquipoLocalId,
    long EquipoVisitanteId,
    DateTime FechaHora,
    long? CanchaId
)
    : IRequest<long>;
