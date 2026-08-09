using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Pais.Get;

/// <summary>
/// Obtiene un Pais por Id.
/// </summary>
public sealed record GetPaisQuery(
    long Id)
    : IRequest<PaisDto>;
