using MediatR;

namespace Touchliga.Application.Commands.Ciudad.Update;

/// <summary>
/// Update Ciudad.
/// </summary>
public sealed record UpdateCiudadCommand(
    long Id,
    string Nombre,
    string Descripcion,
    long PaisId,
    long EstadoId,
    bool Activo
)
    : IRequest<long>;
