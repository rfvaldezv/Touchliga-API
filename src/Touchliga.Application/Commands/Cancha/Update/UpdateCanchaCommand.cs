using MediatR;

namespace Touchliga.Application.Commands.Cancha.Update;

/// <summary>
/// Update Cancha.
/// </summary>
public sealed record UpdateCanchaCommand(
    long Id,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
