using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Jugador.GetAll;

/// <summary>
/// Obtiene la colección de Jugadors.
/// </summary>
public sealed record GetJugadorsQuery()
    : IRequest<IReadOnlyList<JugadorDto>>;
