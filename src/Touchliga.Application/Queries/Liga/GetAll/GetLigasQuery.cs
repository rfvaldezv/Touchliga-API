using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Liga.GetAll;

/// <summary>
/// Obtiene la colección de Ligas.
/// </summary>
public sealed record GetLigasQuery()
    : IRequest<IReadOnlyList<LigaDto>>;
