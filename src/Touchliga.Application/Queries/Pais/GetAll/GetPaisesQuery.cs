using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Pais.GetAll;

/// <summary>
/// Obtiene la colección de Paises.
/// </summary>
public sealed record GetPaisesQuery()
    : IRequest<IReadOnlyList<PaisDto>>;
