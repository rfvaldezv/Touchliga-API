using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Cancha.Get;

/// <summary>
/// Obtiene un Cancha por Id.
/// </summary>
public sealed record GetCanchaQuery(
    long Id)
    : IRequest<CanchaDto>;
