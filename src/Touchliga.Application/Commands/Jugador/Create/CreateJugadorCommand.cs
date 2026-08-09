using MediatR;

namespace Touchliga.Application.Commands.Jugador.Create;

/// <summary>
/// Create Jugador.
/// </summary>
public sealed record CreateJugadorCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
