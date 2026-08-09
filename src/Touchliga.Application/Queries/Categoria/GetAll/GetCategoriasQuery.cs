using MediatR;
using Touchliga.Application.DTOs;

namespace Touchliga.Application.Queries.Categoria.GetAll;

/// <summary>
/// Obtiene la colección de Categorias.
/// </summary>
public sealed record GetCategoriasQuery()
    : IRequest<IReadOnlyList<CategoriaDto>>;
