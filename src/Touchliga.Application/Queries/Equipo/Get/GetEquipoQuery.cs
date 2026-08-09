using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Equipo.Get;

/// <summary>
/// Obtiene un Equipo por Id.
/// </summary>
public sealed record GetEquipoQuery(
    long Id)
    : IRequest<EquipoDto>;
