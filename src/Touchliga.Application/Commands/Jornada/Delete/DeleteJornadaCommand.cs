using MediatR;

namespace Touchliga.Application.Commands.Jornada.Delete;

/// <summary>
/// Elimina un Jornada.
/// </summary>
public sealed record DeleteJornadaCommand(
    long Id)
    : IRequest<Unit>;
