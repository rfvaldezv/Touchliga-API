using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Estandares.GetTablaPosiciones;

/// <summary>
/// Tabla de posiciones de una temporada: suma de puntos de todos
/// los pronósticos ya calificados. Los puntos se calculan partido
/// por partido en cuanto se captura su resultado real, así que la
/// tabla se va actualizando en vivo durante la jornada, no hasta
/// que se cierra.
/// </summary>
public sealed record GetTablaPosicionesQuery(long TemporadaId)
    : IRequest<IReadOnlyList<PosicionDto>>;
