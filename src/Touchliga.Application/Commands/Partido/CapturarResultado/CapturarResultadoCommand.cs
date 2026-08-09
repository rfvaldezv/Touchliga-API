using MediatR;

namespace Touchliga.Application.Commands.Partido.CapturarResultado;

/// <summary>
/// Captura (o corrige) el resultado real de un partido. Lo hace
/// un administrador manualmente.
/// </summary>
public sealed record CapturarResultadoCommand(
    long Id,
    int GolesLocal,
    int GolesVisitante
)
    : IRequest<Unit>;
