using MediatR;

namespace Touchliga.Application.Commands.Pais.Update;

/// <summary>
/// Update Pais.
/// </summary>
public sealed record UpdatePaisCommand(
    long Id,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
