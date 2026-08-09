using MediatR;

namespace Touchliga.Application.Commands.Cancha.Create;

/// <summary>
/// Create Cancha.
/// </summary>
public sealed record CreateCanchaCommand(
    string Codigo,
    string Nombre,
    string Descripcion,
    bool Activo
)
    : IRequest<long>;
