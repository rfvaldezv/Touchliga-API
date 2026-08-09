using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Temporada.Get;

/// <summary>
/// Obtiene un Temporada por Id.
/// </summary>
public sealed record GetTemporadaQuery(
    long Id)
    : IRequest<TemporadaDto>;
