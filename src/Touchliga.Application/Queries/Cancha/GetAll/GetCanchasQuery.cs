using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Cancha.GetAll;

/// <summary>
/// Obtiene la colección de Canchas.
/// </summary>
public sealed record GetCanchasQuery()
    : IRequest<IReadOnlyList<CanchaDto>>;
