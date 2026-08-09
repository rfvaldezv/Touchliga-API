using MediatR;

namespace Touchliga.Application.Commands.Categoria.Delete;

/// <summary>
/// Elimina un Categoria.
/// </summary>
public sealed record DeleteCategoriaCommand(
    long Id)
    : IRequest<Unit>;
