using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Jornada.GetAll;

/// <summary>
/// Obtiene la colección de Jornadas, opcionalmente filtrada por Temporada.
/// </summary>
public sealed record GetJornadasQuery(long? TemporadaId = null)
    : IRequest<IReadOnlyList<JornadaDto>>;
