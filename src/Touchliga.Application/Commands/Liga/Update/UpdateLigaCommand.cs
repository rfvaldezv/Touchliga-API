using MediatR;

namespace Touchliga.Application.Commands.Liga.Update;

/// <summary>
/// Update Liga.
/// </summary>
public sealed record UpdateLigaCommand(
    long Id,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
