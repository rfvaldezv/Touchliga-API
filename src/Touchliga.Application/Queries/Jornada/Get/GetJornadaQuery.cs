using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Jornada.Get;

/// <summary>
/// Obtiene un Jornada por Id.
/// </summary>
public sealed record GetJornadaQuery(
    long Id)
    : IRequest<JornadaDto>;
