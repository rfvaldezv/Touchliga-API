using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Liga.Get;

/// <summary>
/// Obtiene un Liga por Id.
/// </summary>
public sealed record GetLigaQuery(
    long Id)
    : IRequest<LigaDto>;
