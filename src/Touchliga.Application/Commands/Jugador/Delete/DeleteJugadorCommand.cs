using MediatR;

namespace Touchliga.Application.Commands.Jugador.Delete;

/// <summary>
/// Elimina un Jugador.
/// </summary>
public sealed record DeleteJugadorCommand(
    long Id)
    : IRequest<Unit>;
