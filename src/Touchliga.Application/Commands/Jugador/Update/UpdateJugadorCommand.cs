using MediatR;

namespace Touchliga.Application.Commands.Jugador.Update;

/// <summary>
/// Update Jugador.
/// </summary>
public sealed record UpdateJugadorCommand(
    long Id,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
