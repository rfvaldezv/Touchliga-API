using MediatR;

namespace Touchliga.Application.Commands.Ciudad.Create;

/// <summary>
/// Create Ciudad.
/// </summary>
public sealed record CreateCiudadCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    long PaisId,
    long EstadoId,
    bool Activo
)
    : IRequest<long>;
