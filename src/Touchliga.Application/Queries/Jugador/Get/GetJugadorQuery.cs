using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Jugador.Get;

/// <summary>
/// Obtiene un Jugador por Id.
/// </summary>
public sealed record GetJugadorQuery(
    long Id)
    : IRequest<JugadorDto>;
