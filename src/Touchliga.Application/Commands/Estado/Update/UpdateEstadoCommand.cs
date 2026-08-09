using MediatR;

namespace Touchliga.Application.Commands.Estado.Update;

/// <summary>
/// Update Estado.
/// </summary>
public sealed record UpdateEstadoCommand(
    long Id,
    string Nombre,
    string Descripcion,
    long PaisId,
    bool Activo
)
    : IRequest<long>;
