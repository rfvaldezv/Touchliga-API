using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Estado.Get;

/// <summary>
/// Obtiene un Estado por Id.
/// </summary>
public sealed record GetEstadoQuery(
    long Id)
    : IRequest<EstadoDto>;
