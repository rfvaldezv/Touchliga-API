using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Categoria.Get;

/// <summary>
/// Obtiene un Categoria por Id.
/// </summary>
public sealed record GetCategoriaQuery(
    long Id)
    : IRequest<CategoriaDto>;
