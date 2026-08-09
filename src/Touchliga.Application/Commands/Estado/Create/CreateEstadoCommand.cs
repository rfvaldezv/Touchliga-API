using MediatR;

namespace Touchliga.Application.Commands.Estado.Create;

/// <summary>
/// Create Estado.
/// </summary>
public sealed record CreateEstadoCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    long PaisId,
    bool Activo
)
    : IRequest<long>;
