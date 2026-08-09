using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Temporada.GetAll;

/// <summary>
/// Obtiene la colección de Temporadas, opcionalmente filtrada por Liga.
/// </summary>
public sealed record GetTemporadasQuery(long? LigaId = null)
    : IRequest<IReadOnlyList<TemporadaDto>>;
