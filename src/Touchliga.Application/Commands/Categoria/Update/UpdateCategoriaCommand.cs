using MediatR;

namespace Touchliga.Application.Commands.Categoria.Update;

/// <summary>
/// Update Categoria.
/// </summary>
public sealed record UpdateCategoriaCommand(
    long Id,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
