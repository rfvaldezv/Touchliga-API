using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Equipo.GetAll;

/// <summary>
/// Obtiene la colección de Equipos.
/// </summary>
public sealed record GetEquiposQuery()
    : IRequest<IReadOnlyList<EquipoDto>>;
