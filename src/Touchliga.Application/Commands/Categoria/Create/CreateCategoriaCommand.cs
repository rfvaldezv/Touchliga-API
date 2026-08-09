using MediatR;

namespace Touchliga.Application.Commands.Categoria.Create;

/// <summary>
/// Create Categoria.
/// </summary>
public sealed record CreateCategoriaCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
