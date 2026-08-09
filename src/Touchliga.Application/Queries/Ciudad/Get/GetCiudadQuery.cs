using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Ciudad.Get;

/// <summary>
/// Obtiene un Ciudad por Id.
/// </summary>
public sealed record GetCiudadQuery(
    long Id)
    : IRequest<CiudadDto>;
