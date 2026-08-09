using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Estado.GetAll;

/// <summary>
/// Obtiene la colección de Estados.
/// </summary>
public sealed record GetEstadosQuery()
    : IRequest<IReadOnlyList<EstadoDto>>;
