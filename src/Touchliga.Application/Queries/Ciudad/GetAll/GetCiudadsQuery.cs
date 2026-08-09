using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Ciudad.GetAll;

/// <summary>
/// Obtiene la colección de Ciudads.
/// </summary>
public sealed record GetCiudadsQuery()
    : IRequest<IReadOnlyList<CiudadDto>>;
